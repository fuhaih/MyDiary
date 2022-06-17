using PDFiumCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.HDIS.Shared.Models.PdfRender
{
    /// <summary>
    /// PDF bitmap
    /// </summary>
    public class PdfBitmap : IDisposable
    {
        private readonly FpdfBitmapT _pdfBitmap;
        private bool disposedValue;

        public int Width { get; }

        public int Height { get; }

        public PixelFormat Format { get; }

        public int Stride { get; }

        public IntPtr Scan0 { get; }

        public float Scale { get; }

        public RectangleF Viewport { get; }

        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Only call within the dispatcher since dll calls are made.
        /// </summary>
        /// <param name="pdfBitmap"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="dispatcher"></param>
        /// <param name="format"></param>
        /// <param name="scale"></param>
        /// <param name="viewport"></param>
        internal PdfBitmap(
            FpdfBitmapT pdfBitmap,
            int width,
            int height,
            PixelFormat format,
            float scale,
            RectangleF viewport)
        {
            _pdfBitmap = pdfBitmap;
            Scan0 = fpdfview.FPDFBitmapGetBuffer(pdfBitmap);
            Stride = fpdfview.FPDFBitmapGetStride(pdfBitmap);
            Height = height;
            Format = format;
            Scale = scale;
            Viewport = viewport;
            Width = width;
        }

        public Bitmap ToBitmap()
        {
            return new Bitmap(Width, Height, Stride, Format, Scan0);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }
                fpdfview.FPDFBitmapDestroy(_pdfBitmap);
                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~PdfBitmap()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
