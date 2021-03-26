﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Kafka.Common
{
    /// <summary>
    /// A BinaryReader that is BigEndian aware binary reader.
    /// </summary>
    /// <remarks>
    /// Booleans, bytes and byte arrays will be written directly.
    /// All other values will be converted to a byte array in BigEndian byte order and written.
    /// Characters and Strings will all be encoded in UTF-8 (which is byte order independent).
    /// </remarks>
    /// <remarks>
    /// BigEndianBinaryWriter code provided by Zoltu
    /// https://github.com/Zoltu/Zoltu.EndianAwareBinaryReaderWriter
    /// 
    /// The code was modified to provide Kafka specific logic and helper functions.
    /// </remarks>
    public class BigEndianBinaryReader : BinaryReader
    {
        private const int KafkaNullSize = -1;

        public BigEndianBinaryReader(IEnumerable<byte> payload)
            : base(new MemoryStream(payload.ToArray()), Encoding.UTF8)
        {
            base.BaseStream.Position = 0;
        }

        public long Length => base.BaseStream.Length;

        public long Position => base.BaseStream.Position;

        public bool HasData => base.BaseStream.Position < base.BaseStream.Length;

        public bool Available(int dataSize)
        {
            return (base.BaseStream.Length - base.BaseStream.Position) >= dataSize;
        }

        public override decimal ReadDecimal()
        {
            var bytes = GetNextBytesNativeEndian(16);

            var ints = new int[4];
            ints[0] = bytes[0]
                | bytes[1] << 8
                | bytes[2] << 16
                | bytes[3] << 24;
            ints[1] = bytes[4]
                | bytes[5] << 8
                | bytes[6] << 16
                | bytes[7] << 24;
            ints[2] = bytes[8]
                | bytes[9] << 8
                | bytes[10] << 16
                | bytes[11] << 24;
            ints[3] = bytes[12]
                | bytes[13] << 8
                | bytes[14] << 16
                | bytes[15] << 24;

            return new decimal(ints);
        }

        public override float ReadSingle()
        {
            return EndianAwareRead(4, BitConverter.ToSingle);
        }

        public override double ReadDouble()
        {
            return EndianAwareRead(8, BitConverter.ToDouble);
        }

        public override short ReadInt16()
        {
            return EndianAwareRead(2, BitConverter.ToInt16);
        }

        public override int ReadInt32()
        {
            return EndianAwareRead(4, BitConverter.ToInt32);
        }

        public override long ReadInt64()
        {
            return EndianAwareRead(8, BitConverter.ToInt64);
        }

        public override ushort ReadUInt16()
        {
            return EndianAwareRead(2, BitConverter.ToUInt16);
        }

        public override uint ReadUInt32()
        {
            return EndianAwareRead(4, BitConverter.ToUInt32);
        }

        public override ulong ReadUInt64()
        {
            return EndianAwareRead(8, BitConverter.ToUInt64);
        }

        public string ReadInt16String()
        {
            var size = ReadInt16();
            if (size == KafkaNullSize) return null;
            return Encoding.UTF8.GetString(RawRead(size));
        }

        public string ReadIntString()
        {
            var size = ReadInt32();
            if (size == KafkaNullSize) return null;
            return Encoding.UTF8.GetString(RawRead(size));
        }

        public byte[] ReadInt16PrefixedBytes()
        {
            var size = ReadInt16();
            if (size == KafkaNullSize) { return null; }
            return RawRead(size);
        }

        public byte[] ReadIntPrefixedBytes()
        {
            var size = ReadInt32();
            if (size == KafkaNullSize) { return null; }
            return RawRead(size);
        }

        public byte[] ReadToEnd()
        {
            var size = (int)(base.BaseStream.Length - base.BaseStream.Position);
            var buffer = new byte[size];
            base.BaseStream.Read(buffer, 0, size);
            return buffer;
        }

        public byte[] CrcHash()
        {
            var currentPosition = base.BaseStream.Position;
            try
            {
                base.BaseStream.Position = 0;
                return Crc32Provider.ComputeHash(ReadToEnd());
            }
            finally
            {
                base.BaseStream.Position = currentPosition;
            }
        }

        public uint Crc()
        {
            var currentPosition = base.BaseStream.Position;
            try
            {
                base.BaseStream.Position = 0;
                return Crc32Provider.Compute(ReadToEnd());
            }
            finally
            {
                base.BaseStream.Position = currentPosition;
            }
        }

        public byte[] RawRead(int size)
        {
            if (size <= 0) { return new byte[0]; }

            var buffer = new byte[size];

            base.Read(buffer, 0, size);

            return buffer;
        }

        private T EndianAwareRead<T>(int size, Func<byte[], int, T> converter) where T : struct
        {
            Contract.Requires(size >= 0);
            Contract.Requires(converter != null);

            var bytes = GetNextBytesNativeEndian(size);
            return converter(bytes, 0);
        }

        private byte[] GetNextBytesNativeEndian(Int32 count)
        {
            Contract.Requires(count >= 0);
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == count);

            var bytes = GetNextBytes(count);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }

        private byte[] GetNextBytes(int count)
        {
            Contract.Requires(count >= 0);
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == count);

            var buffer = new byte[count];
            var bytesRead = BaseStream.Read(buffer, 0, count);

            if (bytesRead != count)
                throw new EndOfStreamException();

            return buffer;
        }
    }
}
