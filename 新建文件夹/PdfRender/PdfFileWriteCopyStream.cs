using PDFiumCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.HDIS.Shared.Models.PdfRender
{
    class PdfFileWriteCopyStream : FPDF_FILEWRITE_
    {
        public Stream WriteStream { get; }

        public PdfFileWriteCopyStream(Stream writeStream)
        {
            WriteStream = writeStream;
            WriteBlock = CopyToStream;
        }

        private unsafe int CopyToStream(IntPtr pthis, IntPtr pdata, uint size)
        {
            using (var reader = new UnmanagedMemoryStream((byte*)pdata, size))
            {
                reader.CopyTo(WriteStream);
            }
            return 1;
        }
    }
}
