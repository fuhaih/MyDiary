using PDFiumCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SR.HDIS.Shared.Models.PdfRender
{
    /// <summary>
    /// PDF文档
    /// </summary>
    public class PdfDocument : IDisposable
    {
        private readonly FpdfDocumentT _documentInstance;

        private bool disposedValue;

        public int Pages { get; private set; }

        //static PdfDocument()
        //{
        //    Dispatcher = new ThreadDispatcher();
        //    Dispatcher.Start();

        //    // Initialize the library.
        //    Dispatcher.QueueForCompletion(fpdfview.FPDF_InitLibrary);
        //}

        private PdfDocument(FpdfDocumentT documentInstance)
        {
            _documentInstance = documentInstance;
        }

        public static PdfDocument Load(string path, string password)
        {
            int pages = -1;
            fpdfview.FPDF_InitLibrary();
            var result = fpdfview.FPDF_LoadDocument(path, password);
            pages = fpdfview.FPDF_GetPageCount(result);

            if (result == null)
                return null;

            var pdfDocument = new PdfDocument(result)
            {
                Pages = pages
            };

            return pdfDocument;
        }

        public PdfDocument Create()
        {
            var result = fpdf_edit.FPDF_CreateNewDocument();

            if (result == null)
                return null;

            return new PdfDocument(result);
        }

        public PdfPage GetPage(int pageIndex)
        {
            return PdfPage.Create( _documentInstance, pageIndex);
        }

        /// <summary>
        /// Imports pages from another PDF document.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="pageRange">
        /// Pages are 1 based. Pages are separated by commas. Such as "1,3,5-7".
        /// If null, all pages are imported.</param>
        /// <param name="insertIndex">Insertion index is 0 based.</param>
        /// <returns>True on success, false on failure.</returns>
        public bool ImportPages(PdfDocument document, string pageRange, int insertIndex)
        {
            return fpdf_ppo.FPDF_ImportPages(_documentInstance, document._documentInstance, pageRange, insertIndex)==1;
        }

        /// <summary>
        /// Extracts the specified page range into a new PdfDocument.
        /// </summary>
        /// <param name="pageRange">Pages are 1 based. Pages are separated by commas. Such as "1,3,5-7".</param>
        /// <returns>New document with the specified pages.</returns>
        public PdfDocument ExtractPages(string pageRange)
        {
            var newDocument = Create();
            newDocument.ImportPages(this, pageRange, 0);

            return newDocument;
        }

        /// <summary>
        /// Deletes the specified page from the document.
        /// </summary>
        /// <param name="pageIndex">0 based index.</param>
        /// <returns>True on success, false on failure.</returns>
        public bool DeletePage(int pageIndex)
        {
            fpdf_edit.FPDFPageDelete(_documentInstance, pageIndex);
            return true;

        }

        /// <summary>
        /// Saves the current document to the specified file path.
        /// </summary>
        /// <param name="path">Path to save the PdfDocument.</param>
        /// <returns>True on success, false on failure.</returns>
        public bool Save(string path)
        {
            using var fs = new FileStream(path, FileMode.Create);

            return Save(fs);
        }

        /// <summary>
        /// Saves the current document to the passed stream.
        /// </summary>
        /// <param name="stream">Destination stream to write the PdfDocument.</param>
        /// <returns>True on success, false on failure.</returns>
        public bool Save(Stream stream)
        {
            var writer = new PdfFileWriteCopyStream(stream);
            /*
             Flags
            #define FPDF_INCREMENTAL 1
            #define FPDF_NO_INCREMENTAL 2
            #define FPDF_REMOVE_SECURITY 3
             */

            var result = fpdf_save.FPDF_SaveAsCopy(_documentInstance, writer, 1) == 1;

            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    // TODO: 释放托管状态(托管对象)
                }
                fpdfview.FPDF_CloseDocument(_documentInstance);
                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~PdfDocument()
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
