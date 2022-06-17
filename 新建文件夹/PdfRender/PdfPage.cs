using PDFiumCore;
using SR.HDIS.Shared.Models.PdfRender.Actions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SR.HDIS.Shared.Models.PdfRender
{
    /// <summary>
    /// pdf页
    /// </summary>
    public class PdfPage : IDisposable
    {
        private readonly FpdfDocumentT _documentInstance;
        private readonly FpdfPageT _pageInstance;
        private bool _isDisposed = false;
        private bool disposedValue;

        public SizeF Size { get; private set; }

        public int InitialIndex { get; private set; }

        private PdfPage(FpdfDocumentT documentInstance, FpdfPageT pageInstance)
        {
            _documentInstance = documentInstance;
            _pageInstance = pageInstance;
        }

        internal static PdfPage Create(
            FpdfDocumentT documentInstance,
            int pageIndex)
        {
            var loadPageResult = fpdfview.FPDF_LoadPage(documentInstance, pageIndex);
            if (loadPageResult == null)
                throw new Exception($"Failed to open page for page index {pageIndex}.");

            var page = new PdfPage(documentInstance, loadPageResult)
            {
                InitialIndex = pageIndex
            };

            var size = new FS_SIZEF_();
            var result = fpdfview.FPDF_GetPageSizeByIndexF(documentInstance, pageIndex, size);
            var getPageSizeResult = result == 0 ? null : size;

            if (getPageSizeResult == null)
                throw new Exception($"Could not retrieve page size for page index {pageIndex}.");

            page.Size = new SizeF(getPageSizeResult.Width, getPageSizeResult.Height);

            return page;
        }
        /*
        public Task<PdfBitmap> Render(RenderFlags flags)
        {
            return Render(flags, 1);
        }

        public Task<PdfBitmap> Render(RenderFlags flags, float scale)
        {
            return Render(flags, scale, new Rectangle(0, 0, (int) (Size.Width * scale), (int) (Size.Height * scale)));
        }


        public Task<PdfBitmap> Render(RenderFlags flags, float scale, RectangleF viewport)
        {
            return Render(flags, scale, viewport, false, Color.White);
        }
        
        public Task<PdfBitmap> Render(
            RenderFlags flags,
            float scale,
            Viewport viewport,
            bool alpha,
            Color? backgroundColor)
        {
            var translatedRectangle = new RectangleF(
                (int) ((Size.Width / 2 - viewport.Size.Width / 2 + viewport.Origin.X) * scale + viewport.Size.Width / 2 * (scale - 1)),
                (int) ((Size.Height / 2 - viewport.Size.Height / 2 - viewport.Origin.Y) * scale + viewport.Size.Height / 2 * (scale - 1)),
                viewport.Size.Width,
                viewport.Size.Height);

            return Render(flags, scale, translatedRectangle, alpha, backgroundColor);
        }
        */

        public PdfBitmap Render(
            RenderFlags flags,
            float scale,
            RectangleF viewport,
            bool alpha,
            Color? backgroundColor,
            CancellationToken cancellationToken)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(PdfPage));

            if (viewport.IsEmpty)
                throw new ArgumentException("Viewport is empty", nameof(viewport));

            return new RenderPageAction(
                _pageInstance,
                scale,
                viewport,
                flags,
                backgroundColor,
                alpha,
                cancellationToken).Execute();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }
                fpdfview.FPDF_ClosePage(_pageInstance);
                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~PdfPage()
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
