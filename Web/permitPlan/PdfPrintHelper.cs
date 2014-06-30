namespace HawaiiDBEDT.Web.permitPlan
{
    using iTextSharp.text.pdf;
    using System.IO;

    public class PdfPrintHelper
    {
        private Stream outputStream;

        public PdfPrintHelper(Stream outputStream)
        {
            this.outputStream = outputStream;
        }

        public void AnnotateForPrinting(Stream pdfSource)
        {
            using (var pdfReader = new PdfReader(pdfSource))
            using (var pdfStamper = new PdfStamper(pdfReader, this.outputStream))
            {
                pdfStamper.JavaScript = "this.print(true);";
                pdfStamper.Close();
            }
        }
    }
}