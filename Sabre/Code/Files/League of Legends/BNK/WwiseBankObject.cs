﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sabre
{
    public enum WwiseObjectType : byte
    {
        Settings = 1,
        Sound_SFX__Sound_Voice = 2,
        Event_Action = 3,
        Event = 4,
        Random_Container_or_Sequence_Container = 5,
        Switch_Container = 6,
        Actor_Mixer = 7,
        Audio_Bus = 8,
        Blend_Container = 9,
        Music_Segment = 10,
        Music_Track = 11,
        Music_Switch_Container = 12,
        Music_Playlist_Container = 13,
        Attenuation = 14,
        Dialogue_Event = 15,
        Motion_Bus = 16,
        Motion_FX = 17,
        Effect = 18,
        Auxiliary_Bus = 19,
    }

    public class WwiseObject
    {
        public WwiseObjectType objectType;
        // Is null if object type is known
        public byte[] objectData;

        public WwiseObject(WwiseObjectType objectType, byte[] objectData)
        {
            this.objectType = objectType;
            this.objectData = objectData;
        }

        protected byte[] GetObjectBytes(byte[] objectData)
        {
            byte[] objectBytes = null;
            using (MemoryStream mStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(mStream))
                {
                    bw.Write((byte)this.objectType);
                    bw.Write((uint)objectData.Length);
                    bw.Write(objectData);
                }
                objectBytes = mStream.ToArray();
            }
            return objectBytes;
        }

        public virtual byte[] GetBytes()
        {
            return this.GetObjectBytes(this.objectData);
        }
    }

    public class SoundSFXVoiceWwiseObject : WwiseObject
    {
        public uint ID;
        public byte[] unk;
        public StreamSound streamType;
        public uint audioFileID;
        public uint sourceID;
        public SFXVoice soundType;
        public byte[] soundStructure;
        // Assigned only if embedded
        public uint fileOffset;
        public uint fileLength;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <param name="bytesRead">Amount of Bytes read</param>
        /// <param name="bytesRead">Amount of Bytes read</param>
        /// <param name="ID">ID of this object</param>
        /// <param name="audioFileID">ID of the audio file</param>
        /// <param name="sourceID">ID of the source file, if Embedded then = STID Bank ID, if steamed then WEM ID</param>
        /// <param name="length"></param>
        public SoundSFXVoiceWwiseObject(BinaryReader br, uint length) : base(WwiseObjectType.Sound_SFX__Sound_Voice, null)
        {
            // bytesRead is just used just to read sound structure as bytes array
            int bytesRead = 0;
            this.ID = br.ReadUInt32();
            this.unk = br.ReadBytes(4);
            this.streamType = (StreamSound)br.ReadUInt32();
            this.audioFileID = br.ReadUInt32();
            this.sourceID = br.ReadUInt32();
            if (this.streamType == StreamSound.Embedded)
            {
                this.fileOffset = br.ReadUInt32();
                this.fileLength = br.ReadUInt32();
                bytesRead += 8;
            }
            this.soundType = (SFXVoice)br.ReadByte();
            bytesRead += 21;
            this.soundStructure = br.ReadBytes((int)(length - bytesRead));
        }

        public override byte[] GetBytes()
        {
            byte[] objData = null;
            using (MemoryStream mStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(mStream))
                {
                    bw.Write(this.ID);
                    bw.Write(this.unk);
                    bw.Write((uint)this.streamType);
                    bw.Write(this.audioFileID);
                    bw.Write(this.sourceID);
                    if (this.streamType == StreamSound.Embedded)
                    {
                        bw.Write(this.fileOffset);
                        bw.Write(this.fileLength);
                    }
                    bw.Write((byte)this.soundType);
                    bw.Write(this.soundStructure);
                }
                objData = mStream.ToArray();
            }
            return base.GetObjectBytes(objData);
        }

        public enum StreamSound : uint
        {
            Embedded = 0,
            Streamed = 1,
            StreamedWithZeroLatency = 2,
        }

        public enum SFXVoice : byte
        {
            SFX = 0,
            Voice = 1,
        }
    }

    public class EventActionWwiseObject : WwiseObject
    {
        public uint ID;
        public byte scope;
        public byte type;
        public uint gameObject;
        public byte[] additional;

        public EventActionWwiseObject(BinaryReader br, uint length) : base(WwiseObjectType.Event_Action, null)
        {
            this.ID = br.ReadUInt32();
            this.scope = br.ReadByte();
            this.type = br.ReadByte();
            this.gameObject = br.ReadUInt32();
            this.additional = br.ReadBytes((int)length - 10);
        }

        public override byte[] GetBytes()
        {
            byte[] objData = null;
            using (MemoryStream mStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(mStream))
                {
                    bw.Write(this.ID);
                    bw.Write(this.scope);
                    bw.Write(this.type);
                    bw.Write(this.gameObject);
                    bw.Write(this.additional);
                }
                objData = mStream.ToArray();
            }
            return base.GetObjectBytes(objData);
        }
    }

    public class EventWwiseObject : WwiseObject
    {
        public uint ID;
        public List<uint> eventActionList = new List<uint>();

        public EventWwiseObject(BinaryReader br) : base(WwiseObjectType.Event, null)
        {
            this.ID = br.ReadUInt32();
            uint eventActionsCount = br.ReadUInt32();
            for (uint i = 0; i < eventActionsCount; i++)
            {
                this.eventActionList.Add(br.ReadUInt32());
            }
        }

        public override byte[] GetBytes()
        {
            byte[] objData = null;
            using (MemoryStream mStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(mStream))
                {
                    bw.Write(this.ID);
                    bw.Write((uint)this.eventActionList.Count);
                    foreach (uint eventActionID in this.eventActionList)
                    {
                        bw.Write(eventActionID);
                    }
                }
                objData = mStream.ToArray();
            }
            return base.GetObjectBytes(objData);
        }
    }
}
