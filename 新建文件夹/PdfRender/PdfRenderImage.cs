using PDFiumCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.HDIS.Shared.Models.PdfRender
{
    /// <summary>
    /// PDF图片绘制类
    /// </summary>
    public class PdfRenderImage : IDisposable
    {
        PdfDocument _document;
        private bool disposedValue;

        public PdfRenderImage(PdfDocument document)
        {
            this._document = document;
        }

        public static  PdfRenderImage Load(string path) {
            var document = PdfDocument.Load(path, null);
            return new PdfRenderImage(document);
        }
        public IEnumerable<Bitmap> GetImages()
        {
            for (var index = 0; index < _document.Pages; index++)
            {
                using var page = _document.GetPage(index);
                var iterations = 2;
                //await using var page = await drawing.GetPage(0);
                var viewport = new Rectangle(500, 50, 500, 500);

                for (int i = 1; i < iterations; i++)
                {
                    /*
                     * new BitmapClip(
                            viewport.Left * scale + viewport.Width / 2 * (scale - 1), 
                            viewport.Top * scale + viewport.Height / 2 * (scale - 1), 
                            (page.Size.Width - viewport.Width - viewport.Left) * scale + viewport.Width / 2 * (scale - 1), 
                            ((page.Size.Height - viewport.Height - viewport.Top) * scale) + viewport.Height / 2 * (scale - 1)), 
                     */
                    float scale = i;
                    Point center = new Point(0, 0);
                    Size size = new Size(1920, 1080);

                    using var result = page.Render(RenderFlags.RenderAnnotations, scale,
                        //这个是绘制固定大小的，页面很大的时候会有异常
                        //new Rectangle((int)((page.Size.Width / 2 - size.Width / 2 + center.X) * scale + size.Width / 2 * (scale - 1)),
                        //    (int)((page.Size.Height / 2 - size.Height / 2 - center.Y) * scale + size.Height / 2 * (scale - 1)),
                        //    size.Width,
                        //    size.Height),

                        //直接安装页面大小绘制
                        new Rectangle(0,0,
                            (int)page.Size.Width,
                            (int)page.Size.Height),
                        false, Color.White, default);
                    //Console.WriteLine($"{sw.ElapsedMilliseconds:##,###}");
                    //result.ToBitmap().Save($"test{i}_{index}.png", ImageFormat.Png);
                    yield return result.ToBitmap();
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }
                _document.Dispose();
                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~PdfRenderImage()
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
