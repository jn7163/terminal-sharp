﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Terminal
{
    class TerminalClient
    {
    }

    class NetworkByteWriter
    {
        BinaryWriter bw;
        public NetworkByteWriter(Stream input)
        {
            bw = new BinaryWriter(input);
        }

        public void WriteByte(Byte data)
        {
            bw.Write(data);
        }

        public void WriteBytes(Byte[] data)
        {
            bw.Write(data);
        }

        public void WriteBoolean(Boolean data)
        {
            bw.Write(data);
        }

        public void WriteUInt32(UInt32 data)
        {
            byte[] result = BitConverter.GetBytes(data);
            Array.Reverse(result);
            bw.Write(result);
        }
        public void WriteUInt64(UInt64 data)
        {
            byte[] result = BitConverter.GetBytes(data);
            Array.Reverse(result);
            bw.Write(result);
        }
        public void WriteString(String data)
        {
            byte[] result = Encoding.UTF8.GetBytes(data);
            WriteUInt32((UInt32)result.Length);
            bw.Write(result);
        }
        public void WriteNameList(String[] data)
        {
            String result = String.Join(",", data);
            WriteString(result);
        }
        public void WriteMPInt(Byte[] data)
        {
            WriteUInt32((UInt32)data.Length);
            bw.Write(data);
        }
        public void Flush()
        {
            bw.Flush();
        }

    }

    class NetworkByteReader
    {
        BinaryReader br;
        public NetworkByteReader(Stream input)
        {
            br = new BinaryReader(input);
        }

        public Byte ReadByte()
        {
            return br.ReadByte();
        }

        public Byte[] ReadBytes(UInt32 size)
        {
            return br.ReadBytes((int)size);
        }

        public Boolean ReadBoolean()
        {
            return br.ReadBoolean();
        }

        public UInt32 ReadUInt32()
        {
            byte[] result = br.ReadBytes(4);
            Array.Reverse(result);
            return BitConverter.ToUInt32(result, 0);
        }

        public UInt64 ReadUInt64()
        {
            byte[] result = br.ReadBytes(8);
            Array.Reverse(result);
            return BitConverter.ToUInt64(result, 0);
        }

        public Byte[] ReadMPInt()
        {
            uint size = ReadUInt32();
            return br.ReadBytes((int)size);
        }

        public String ReadString()
        {
            uint size = ReadUInt32();
            byte[] result = br.ReadBytes((int)size);
            return Encoding.UTF8.GetString(result);
        }
        public String[] ReadNameList()
        {
            uint size = ReadUInt32();
            byte[] result = br.ReadBytes((int)size);
            string list = Encoding.UTF8.GetString(result);
            char[] x = { ',' };
            return list.Split(x);
        }
    }
}