// Copyright (c) 2012-2015 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dicom.IO.Buffer
{
    public class CompositeByteBuffer : IByteBuffer
    {
        public CompositeByteBuffer(params IByteBuffer[] buffers)
        {
            Buffers = new List<IByteBuffer>(buffers);
        }

        public IList<IByteBuffer> Buffers { get; private set; }

        public bool IsMemory
        {
            get
            {
                return true;
            }
        }

        public uint Size
        {
            get
            {
                return (uint)Buffers.Sum(x => x.Size);
            }
        }
        //直接写入target，避免调用 public byte[] Data 占用内存
        public void writeToTarget(IByteTarget target)
        {
            foreach (IByteBuffer buffer in Buffers)
            {
                if (buffer is PInvokeByteBuffer)
                {
                    var pb = buffer as PInvokeByteBuffer;

                    var rowBuffer = new byte[pb.RowBufferSize];
                    for (int i = 0; i < pb.Height; i++)
                    {
                        pb.GetByteRange(i, rowBuffer);
                        target.Write(rowBuffer, 0, (uint)rowBuffer.Length);
                    }
                    rowBuffer = null;
                }
                else
                {
                    var data = buffer.Data;
                    target.Write(data, 0, (uint)data.Length);
                    
                }

            } 
        }
        public byte[] Data
        {
            get
            {
                byte[] data = new byte[Size];
                int offset = 0;
                foreach (IByteBuffer buffer in Buffers)
                {
                    System.Buffer.BlockCopy(buffer.Data, 0, data, offset, (int)buffer.Size);
                    offset += (int)buffer.Size;
                }
                return data;
            }
        }

        public byte[] GetByteRange(int offset, int count)
        {
            int pos = 0;
            for (; pos < Buffers.Count && offset > Buffers[pos].Size; pos++) offset -= (int)Buffers[pos].Size;

            int offset2 = 0;
            byte[] data = new byte[count];
            for (; pos < Buffers.Count && count > 0; pos++)
            {
                int remain = Math.Min((int)Buffers[pos].Size - offset, count);

                if (Buffers[pos].IsMemory)
                {
                    try
                    {
                        System.Buffer.BlockCopy(Buffers[pos].Data, offset, data, offset2, remain);
                    }
                    catch (Exception)
                    {
                        data = Buffers[pos].Data.ToArray();
                    }

                }

                else
                {
                    byte[] temp = Buffers[pos].GetByteRange(offset, remain);
                    System.Buffer.BlockCopy(temp, 0, data, offset2, remain);
                }

                count -= remain;
                offset2 += remain;
                if (offset > 0) offset = 0;
            }

            return data;
        }


        public void writeToTarget(IByteTarget target, IByteBuffer buff)
        {
            foreach (IByteBuffer buffer in Buffers)
            {
                if (buffer is StreamByteBuffer || buffer is FileByteBuffer || buffer is PInvokeByteBuffer)
                {
                    buffer.writeToTarget(target, buff);
                }
                else
                {
                    target.Write(buffer.Data, 0, buffer.Size);
                }

            } 
        }
    }
}
