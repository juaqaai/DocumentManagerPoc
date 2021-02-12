using DocumentManagerPoc.PdfWriter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentManagerPoc.PdfWriterTest
{
    [TestClass]
    public class GemBoxPdfWriterTest : PdfWriterTestBase
    {
        [TestMethod]
        public void CreatePdfFileTest()
        {
            var competitionResult = GetCompetitionResult();

            var pdfWriter = new GemBoxPdfWriter("GemBoxResults");

            InvokeAction(pdfWriter.CreatePdfFile, competitionResult, "GemBoxPdf egyetlen pdf fájl látrehozásának ideje");
        }

        [TestMethod]
        public void Create10000PdfFileTest()
        {
            var competitionResult = GetCompetitionResult();

            var pdfWriter = new GemBoxPdfWriter("GemBoxResults");

            InvokeAction(pdfWriter.Create10000PdfFile, competitionResult, "GemBoxPdf 10000 pdf fájl látrehozásának ideje");
        }
    }
}
