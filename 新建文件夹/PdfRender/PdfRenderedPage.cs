using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.HDIS.Shared.Models.PdfRender
{
    /// <summary>
    /// pdf页
    /// </summary>
    public class PdfRenderedPage : IDisposable
    {
        private bool disposedValue;

        public PdfBitmap PdfBitmap { get; }
        //public BitmapSource BitmapSource { get; }

        public PdfRenderedPage(PdfBitmap pdfBitmap)
        {
            PdfBitmap = pdfBitmap ?? throw new ArgumentNullException(nameof(pdfBitmap));
            /*
            BitmapSource = BitmapSource.Create(
                pdfBitmap.Width,
                pdfBitmap.Height,
                72,
                72,
                PixelFormats.Bgra32,
                null,
                pdfBitmap.Scan0,
                pdfBitmap.Stride * pdfBitmap.Height,
                pdfBitmap.Stride);*/
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }
                PdfBitmap.Dispose();
                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~PdfRenderedPage()
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
