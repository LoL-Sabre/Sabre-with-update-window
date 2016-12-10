﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sabre
{
    class SKNFile
    {
        public UInt32 Zero;
        public BinaryReader br;
        public Header header;
        public BoundingBox boundingbox;
        public List<Material> Materials = new List<Material>();
        public List<UInt16> Indices = new List<UInt16>();
        public List<Vertex> Vertices = new List<Vertex>();
        public UInt32 IndCount;
        public UInt32 VertCount;
        public SKNFile(string fileLocation)
        {
            br = new BinaryReader(File.Open(fileLocation, FileMode.Open));
            header = new Header(br);
            for(int i = 0; i < header.NumOfMaterials; i++)
            {
                Materials.Add(new Material(br));
            }
            Zero = br.ReadUInt32();
            IndCount = br.ReadUInt32() + 6;
            VertCount = br.ReadUInt32();
            if(header.Version == 4)
            {
                boundingbox = new BoundingBox(br);
            }
            for(int i = 0; i < IndCount; i++)
            {
                Indices.Add(br.ReadUInt16());
            }
            for(int i = 0; i < VertCount; i++)
            {
                Vertices.Add(new Vertex(br, (int)boundingbox.AdditionalsCount));
            }
        }
        public class Header
        {
            public byte[] Magic;
            public UInt16 Version;
            public UInt16 NumOfObjects;
            public UInt32 NumOfMaterials;
            public Header(BinaryReader br)
            {
                Magic = br.ReadBytes(4);
                Version = br.ReadUInt16();
                NumOfObjects = br.ReadUInt16();
                NumOfMaterials = br.ReadUInt32();
            }
        }
        public class Material
        {
            public string Name;
            public UInt32 StartVertex;
            public UInt32 NumOfVertices;
            public UInt32 StartIndex;
            public UInt32 NumOfIndices;
            public Material(BinaryReader br)
            {
                Name = Encoding.ASCII.GetString(br.ReadBytes(64));
                StartVertex = br.ReadUInt32();
                NumOfVertices = br.ReadUInt32();
                StartIndex = br.ReadUInt32();
                NumOfIndices = br.ReadUInt32();
            }
        }
        public class BoundingBox
        {
            public UInt32 VertexSize;
            public UInt32 AdditionalsCount;
            public float MinX;
            public float MinY;
            public float MinZ;
            public float MaxX;
            public float MaxY;
            public float MaxZ;
            public float Radius;
            public BoundingBox(BinaryReader br)
            {
                VertexSize = br.ReadUInt32();
                AdditionalsCount = br.ReadUInt32();
                MinX = br.ReadSingle();
                MinY = br.ReadSingle();
                MinZ = br.ReadSingle();
                MaxX = br.ReadSingle();
                MaxY = br.ReadSingle();
                MaxZ = br.ReadSingle();
                Radius = br.ReadSingle();
            }
        }
        public class Vertex
        {
            public long Offset;
            public float[] Position = new float[3];
			public UInt32 BoneIndex;
			public float[] Weights = new float[4]; //4 orig
            public UInt32 BoneHash;
            public float WeightValue;
			public float[] Normal = new float[3];
			public float[] UV = new float[2];
            public List<Int32> Additionals = new List<Int32>();
            public Vertex(BinaryReader br, int AdditionalCount)
            {
                Offset = br.BaseStream.Position;
                for(int i = 0; i < 3; i++)
                {
                    Position[i] = br.ReadSingle();
                }
                BoneIndex = br.ReadUInt32();
                for(int i = 0; i < 2; i++) 
                {
                    Weights[i] = br.ReadSingle();
                }
                BoneHash = br.ReadUInt32(); //?
                WeightValue = br.ReadSingle();
                for(int i = 0; i < 3; i++)
                {
                    Normal[i] = br.ReadSingle();
                }
                for(int i = 0; i < 2; i++)
                {
                    UV[i] = br.ReadSingle();
                }
                for(int i = 0; i < AdditionalCount; i++)
                {
                    Additionals.Add(br.ReadInt32());
                }
            }
        }
    }
}
