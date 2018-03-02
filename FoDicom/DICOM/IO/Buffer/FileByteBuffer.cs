// Copyright (c) 2012-2015 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).

namespace Dicom.IO.Buffer
{
    public sealed class FileByteBuffer : IByteBuffer
    {
        public FileByteBuffer(IFileReference file, long Position, uint Length)
        {
            this.File = file;
            this.Position = Position;
            this.Size = Length;
        }

        public bool IsMemory
        {
            get
            {
                return false;
            }
        }

        public IFileReference File { get; private set; }

        public long Position { get; private set; }

        public uint Size { get; private set; }

        public byte[] Data
        {
            get
            {
                return File.GetByteRange((int)Position, (int)Size);
            }
        }




        public byte[] GetByteRange(int offset, int count)
        {
            return File.GetByteRange((int)Position + offset, count);
        }

        public void writeToTarget(IByteTarget _target, IByteBuffer fb)
        {
            const int seg = 4096000;
            int gcd = (int)(fb.Size / seg);
            int gcf = (int)(fb.Size % seg);

            if (gcd == 0)
            {
                _target.Write(fb.Data, 0, fb.Size);
            }
            else
            {
                for (int i = 0; i < gcd; i++)
                {

                    var rgBuffer = fb.GetByteRange(i * seg, seg);
                    _target.Write(rgBuffer, 0, seg);
                    rgBuffer = null;
                }

                if (gcf > 0)
                {

                    var rgBuffer = fb.GetByteRange(gcd * seg, gcf);

                    _target.Write(rgBuffer, 0, (uint)gcf);

                    rgBuffer = null;
                }
            }
        }
    }
}
