using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dicom.IO.Buffer
{
    public sealed class PInvokeByteBuffer : IByteBuffer
    {

        private readonly IntPtr _pxlData;
        private readonly int _w;
        private readonly int _h;
        private readonly bool _bColor;
        public PInvokeByteBuffer(IntPtr pxlData, int widht, int height, bool isColor = false)
        {
            _pxlData = pxlData;
            _w = widht;
            _h = height;
            _bColor = isColor;
        }


        public bool IsMemory
        {
            get { return true; }
        }

        public uint Size
        {
            get
            {
                return (uint)(_w * _h * 3);
                ;
            }
        }

        public byte[] Data
        {
            get { throw new NotImplementedException(); }
        }


        public IntPtr PixelData
        {
            get { return _pxlData; }
        }

        public int Width
        {
            get { return _w; }
        }

        public int Height
        {
            get { return _h; }
        }

        public int RowBufferSize
        {
            get
            {
                if (_bColor)
                {
                    return _w * 3;
                }
                else
                {
                    return _w;
                }
            }
        }

        public void GetByteRange(int row, byte[] buffer)
        {
            if (false == _bColor)
            {
                GetGreyBytesEx(row, buffer);

            }
            else
            {
                GetColorBytesEx(row, buffer);
            }
        }


        private const int SrcComponents = 3;


        private unsafe void GetGreyBytesEx(int rowIndex,
            byte[] dataBuffer)
        {
            GCHandle handle = GCHandle.Alloc(dataBuffer, GCHandleType.Pinned);
            IntPtr pointer = handle.AddrOfPinnedObject();
            var dstLine = (byte*)pointer.ToPointer();
            var srcLine = (byte*)_pxlData.ToPointer();

            srcLine += rowIndex * _w * SrcComponents;
            for (int j = 0; j < _w; j++)
            {
                var pixel = srcLine + j * SrcComponents;
                int grey = (int)(pixel[0] * 0.3 + pixel[1] * 0.59 + pixel[2] * 0.11);
                dstLine[j] = (byte)grey;
            }
            pointer = IntPtr.Zero;
            handle.Free();

        }


        private unsafe void GetColorBytesEx(int rowIndex,
            byte[] dataBuffer)
        { 
            GCHandle handle = GCHandle.Alloc(dataBuffer, GCHandleType.Pinned);
            IntPtr pointer = handle.AddrOfPinnedObject();
             
            var dstLine = (byte*)pointer.ToPointer();
            var srcLine = (byte*)_pxlData.ToPointer(); 

            srcLine += rowIndex * _w * SrcComponents;
            for (int j = 0; j < _w; j++)
            {
                var srcPixel = srcLine + j * SrcComponents;
                var dstPixel = dstLine + j * 3;

                //convert from bgr to rgb
                dstPixel[0] = srcPixel[0];
                dstPixel[1] = srcPixel[1];
                dstPixel[2] = srcPixel[2];
            } 
            pointer = IntPtr.Zero;
            handle.Free();


        }



        public byte[] GetByteRange(int offset, int count)
        {
            throw new NotImplementedException();
        }


        public void writeToTarget(IByteTarget _target, IByteBuffer buffer)
        {
            var pb = buffer as PInvokeByteBuffer;
            var rowBuffer = new byte[pb.RowBufferSize];
            for (int i = 0; i < pb.Height; i++)
            {
                pb.GetByteRange(i, rowBuffer);
                _target.Write(rowBuffer, 0, (uint)rowBuffer.Length);
            }
            rowBuffer = null;
        }
    }
}
