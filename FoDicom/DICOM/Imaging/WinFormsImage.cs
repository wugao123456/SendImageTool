// Copyright (c) 2012-2015 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).

namespace Dicom.Imaging
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;

    using Dicom.Imaging.Render;
    using Dicom.IO;
    using System;

    /// <summary>
    /// Convenience class for non-generic access to <see cref="WinFormsImage"/> image objects.
    /// </summary>
    public static class WinFormsImageExtensions
    {
        /// <summary>
        /// Convenience method to access WinForms <see cref="IImage"/> instance as WinForms <see cref="Bitmap"/>.
        /// The returned <see cref="Bitmap"/> will be disposed when the <see cref="IImage"/> is disposed.
        /// </summary>
        /// <param name="image"><see cref="IImage"/> object.</param>
        /// <returns><see cref="Bitmap"/> contents of <paramref name="image"/>.</returns>
        public static Bitmap AsBitmap(this IImage image)
        {
            return image.As<Bitmap>();
        }
    }

    /// <summary>
    /// <see cref="IImage"/> implementation of a Windows Forms <see cref="Bitmap"/>.
    /// </summary>
    public sealed partial class WinFormsImage : ImageDisposableBase<Bitmap>
    {
        #region CONSTRUCTORS

        /// <summary>
        /// Initializes an instance of the <see cref="WinFormsImage"/> object.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        public WinFormsImage(int width, int height)
            : base(width, height, new PinnedIntArray(width * height), null)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="WinFormsImage"/> object.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="pixels">Pixel array.</param>
        /// <param name="image">Bitmap image.</param>
        private WinFormsImage(int width, int height, PinnedIntArray pixels, Bitmap image)
            : base(width, height, pixels, image)
        {
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Renders the image given the specified parameters.
        /// </summary>
        /// <param name="components">Number of components.</param>
        /// <param name="flipX">Flip image in X direction?</param>
        /// <param name="flipY">Flip image in Y direction?</param>
        /// <param name="rotation">Image rotation.</param>
        public override void Render(int components, bool flipX, bool flipY, int rotation)
        {
            var format = components == 4 ? PixelFormat.Format32bppArgb : PixelFormat.Format32bppRgb;
            var stride = GetStride(this.width, format);

            this.image = new Bitmap(this.width, this.height, stride, format, this.pixels.Pointer);

            var rotateFlipType = GetRotateFlipType(flipX, flipY, rotation);
            if (rotateFlipType != RotateFlipType.RotateNoneFlipNone)
            {
                this.image.RotateFlip(rotateFlipType);
            }
        }

        public override bool Render(int components, bool bRorate90, string saveToPath)
        {
            //  var format = components == 4 ? PixelFormat.Format32bppArgb : PixelFormat.Format32bppRgb;
            var _pixels = this.pixels;

            using (var fs = new System.IO.FileStream(saveToPath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            using (var bw = new System.IO.BinaryWriter(fs))
            {
                var bheader = new DcmBITMAPFILEHEADER();
                bheader.bfOffBits = 54;
                bheader.bfReserved1 = 0;
                bheader.bfReserved2 = 0;
                bheader.bfSize = (uint)(54 + _pixels.ByteSize);
                bheader.bfType = 19778;

                var binfo = new DcmBITMAPINFOHEADER();
                binfo.biBitCount = 32;
                binfo.biClrImportant = 0;
                binfo.biClrUsed = 0;
                binfo.biCompression = DcmBitmapCompressionMode.BI_RGB;
                if (bRorate90)
                {
                    //----旋转90度, 其它参数不变
                    binfo.biHeight = this.width;
                    binfo.biWidth = this.height;

                }
                else
                {
                    binfo.biWidth = this.width;
                    binfo.biHeight = this.height;
                }

                binfo.biPlanes = 1;
                binfo.biSize = 40;
                binfo.biSizeImage = 0;
                binfo.biXPelsPerMeter = 3978;
                binfo.biYPelsPerMeter = 3978;
                var hd = DcmStructToBytes(bheader);
                var bd = DcmStructToBytes(binfo);

                bw.Write(hd);
                bw.Write(bd);

                if (false == bRorate90)
                {
                    for (int i = this.height; i > 0; i--)
                    {
                        //int[] rawData = new int[ScaledData.Width]; 
                        //Buffer.BlockCopy(_pixels.Data, (i - 1) * ScaledData.Width, rawData, 0, ScaledData.Width); 
                        for (int j = 0; j < this.width; j++)
                        {
                            int ax = _pixels[(i - 1) * this.width + j];
                            byte[] ar = BitConverter.GetBytes(ax);
                            bw.Write(ar);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.width; i++)
                    {
                        for (int j = 0; j < this.height; j++)
                        {
                            int aw = _pixels[j * this.width + i];
                            byte[] ar = BitConverter.GetBytes(aw);
                            bw.Write(ar);
                        }
                    }

                }
                bw.Flush();
                bw.Close();
                fs.Close();
                return true;
            }


        }

        /// <summary>
        /// Draw graphics onto existing image.
        /// </summary>
        /// <param name="graphics">Graphics to draw.</param>
        public override void DrawGraphics(IEnumerable<IGraphic> graphics)
        {
            using (var g = Graphics.FromImage(this.image))
            {
                foreach (var graphic in graphics)
                {
                    var layer = graphic.RenderImage(null).As<Image>();
                    g.DrawImage(layer, graphic.ScaledOffsetX, graphic.ScaledOffsetY, graphic.ScaledWidth, graphic.ScaledHeight);
                }
            }
        }

        /// <summary>
        /// Creates a deep copy of the image.
        /// </summary>
        /// <returns>Deep copy of this image.</returns>
        public override IImage Clone()
        {
            return new WinFormsImage(
                this.width,
                this.height,
                new PinnedIntArray(this.pixels.Data),
                this.image == null ? null : new Bitmap(this.image));
        }

        private static int GetStride(int width, PixelFormat format)
        {
            var bitsPerPixel = ((int)format & 0xff00) >> 8;
            var bytesPerPixel = (bitsPerPixel + 7) / 8;
            return 4 * ((width * bytesPerPixel + 3) / 4);
        }

        private static RotateFlipType GetRotateFlipType(bool flipX, bool flipY, int rotation)
        {
            if (flipX && flipY)
            {
                switch (rotation)
                {
                    case 90:
                        return RotateFlipType.Rotate90FlipXY;
                    case 180:
                        return RotateFlipType.Rotate180FlipXY;
                    case 270:
                        return RotateFlipType.Rotate270FlipXY;
                    default:
                        return RotateFlipType.RotateNoneFlipXY;
                }
            }
            else if (flipX)
            {
                switch (rotation)
                {
                    case 90:
                        return RotateFlipType.Rotate90FlipX;
                    case 180:
                        return RotateFlipType.Rotate180FlipX;
                    case 270:
                        return RotateFlipType.Rotate270FlipX;
                    default:
                        return RotateFlipType.RotateNoneFlipX;
                }
            }
            else if (flipY)
            {
                switch (rotation)
                {
                    case 90:
                        return RotateFlipType.Rotate90FlipY;
                    case 180:
                        return RotateFlipType.Rotate180FlipY;
                    case 270:
                        return RotateFlipType.Rotate270FlipY;
                    default:
                        return RotateFlipType.RotateNoneFlipY;
                }
            }
            else
            {
                switch (rotation)
                {
                    case 90:
                        return RotateFlipType.Rotate90FlipNone;
                    case 180:
                        return RotateFlipType.Rotate180FlipNone;
                    case 270:
                        return RotateFlipType.Rotate270FlipNone;
                    default:
                        return RotateFlipType.RotateNoneFlipNone;
                }
            }
        }

        #endregion

        public override bool Render(int components, System.IO.MemoryStream bw, out int w, out int h, bool bRorate90)
        {
            w = this.width;
            h = this.height;
            var _pixels = this.pixels;
            {


                var bheader = new DcmBITMAPFILEHEADER();
                bheader.bfOffBits = 54;
                bheader.bfReserved1 = 0;
                bheader.bfReserved2 = 0;
                bheader.bfSize = (uint)(54 + _pixels.ByteSize);
                bheader.bfType = 19778;

                var binfo = new DcmBITMAPINFOHEADER();
                binfo.biBitCount = 32;
                binfo.biClrImportant = 0;
                binfo.biClrUsed = 0;
                binfo.biCompression = DcmBitmapCompressionMode.BI_RGB;
                if (bRorate90)
                {
                    //----旋转90度, 其它参数不变
                    h = this.width;
                    w = this.height;

                }
                else
                {
                    w = this.width;
                    h = this.height;
                }

                binfo.biPlanes = 1;
                binfo.biSize = 40;
                binfo.biSizeImage = 0;
                binfo.biXPelsPerMeter = 3978;
                binfo.biYPelsPerMeter = 3978;
                var hd = DcmStructToBytes(bheader);
                var bd = DcmStructToBytes(binfo);

                bw.Write(hd, 0, hd.Length);
                bw.Write(bd, 0, bd.Length);



                

                if (false == bRorate90)
                {
                    for (int i = this.height; i > 0; i--)
                    {
                        //int[] rawData = new int[ScaledData.Width]; 
                        //Buffer.BlockCopy(_pixels.Data, (i - 1) * ScaledData.Width, rawData, 0, ScaledData.Width); 
                        for (int j = 0; j < this.width; j++)
                        {
                            int ax = _pixels[(i - 1) * this.width + j];
                            byte[] ar = BitConverter.GetBytes(ax);
                            bw.Write(ar, 0, 3);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.width; i++)
                    {
                        for (int j = 0; j < this.height; j++)
                        {
                            int aw = _pixels[j * this.width + i];
                            byte[] ar = BitConverter.GetBytes(aw);
                            bw.Write(ar, 0, 3);
                        }

                    }


                }
                bw.Flush();

                return true;

            }
        }
    }
}