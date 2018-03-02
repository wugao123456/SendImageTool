using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Dicom.Imaging
{
    public   class FileSystemImage : IFileImage
    {
       
        private string filePath { get; set; }
        public System.IO.Stream fileStream { get; private set; }

        public FileSystemImage(int width, int height, string saveToBmp)
        {
            filePath = saveToBmp;
            fileStream = File.Open(saveToBmp, FileMode.Create, FileAccess.Write, FileShare.None);
            using (var bw = new System.IO.BinaryWriter(fileStream, Encoding.UTF8, true))
            {
                var bheader = new DcmBITMAPFILEHEADER();
                bheader.bfOffBits = 54;
                bheader.bfReserved1 = 0;
                bheader.bfReserved2 = 0;
                bheader.bfSize = (uint)(54 + width * height * 4);
                bheader.bfType = 19778;

                var binfo = new DcmBITMAPINFOHEADER();
                binfo.biBitCount = 32;
                binfo.biClrImportant = 0;
                binfo.biClrUsed = 0;
                binfo.biCompression = DcmBitmapCompressionMode.BI_RGB;

                binfo.biWidth = width;
                binfo.biHeight = height;
                binfo.biPlanes = 1;
                binfo.biSize = 40;
                binfo.biSizeImage = 0;
                binfo.biXPelsPerMeter = 3978;
                binfo.biYPelsPerMeter = 3978;
                var hd = DcmStructToBytes(bheader);
                var bd = DcmStructToBytes(binfo);
                bw.Write(hd);
                bw.Write(bd);
            }
        }

        public Stream Pixels
        {
            get { return fileStream; }
        }

        public void Flush()
        {
            fileStream.Flush();
        }

        ~FileSystemImage()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected bool disposed;
        /// <summary>
        /// Dispose resources.
        /// </summary>
        /// <param name="disposing">Dispose mode?</param>
        protected   void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (fileStream != null)
            {
                fileStream.Close();
                fileStream.Dispose();
            }

            this.disposed = true;
        }


        public static byte[] DcmStructToBytes<T>(T structObj, int index = 0) where T : struct
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(structObj);
            //创建byte数组
            byte[] bytes = new byte[size + index];
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, index, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;
        }
    }
}