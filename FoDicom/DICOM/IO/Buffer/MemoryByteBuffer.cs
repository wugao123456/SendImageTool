// Copyright (c) 2012-2015 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).

using System;

namespace Dicom.IO.Buffer
{
    public sealed class MemoryByteBuffer : IByteBuffer
    {
        public MemoryByteBuffer(byte[] Data)
        {
            int len = Data.Length;
            this.Data = new byte[len];
            System.Buffer.BlockCopy(Data, 0, this.Data, 0, len);
        }

        public bool IsMemory
        {
            get
            {
                return true;
            }
        }

        public byte[] Data { get; private set; }

        public uint Size
        {
            get
            {
                return (uint)Data.Length;
            }
        }

        public byte[] GetByteRange(int offset, int count)
        {
            if (offset == 0 && count == Size) return Data;

            byte[] buffer = new byte[count];
            Array.Copy(Data, offset, buffer, 0, count);
            return buffer;
        }


        public void writeToTarget(IByteTarget target, IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class MemoryStreamBuffer : IByteBuffer
    {
        private System.IO.MemoryStream _ms;
        public MemoryStreamBuffer(System.IO.MemoryStream ms)
        {
            if (ms == null)
                throw new System.ArgumentNullException();
             

            _ms = ms;


        }

        public bool IsMemory
        {
            get
            {
                return true;
            }
        }

        public byte[] Data { get { return _ms.ToArray(); } }

        public uint Size
        {
            get
            {
                return (uint)_ms.Length;
            }
        }

        public byte[] GetByteRange(int offset, int count)
        {
            if (offset == 0 && count == Size) return Data;

            byte[] buffer = new byte[count];
            _ms.Read(buffer, offset, count);
            return buffer;
        }


        public void writeToTarget(IByteTarget target)
        {
            throw new NotImplementedException();
        }


        public void writeToTarget(IByteTarget target, IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }

}
