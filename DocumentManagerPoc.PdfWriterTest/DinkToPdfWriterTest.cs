using DocumentManagerPoc.PdfWriter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentManagerPoc.PdfWriterTest
{
    [TestClass]
    public class DinkToPdfWriterTest : PdfWriterTestBase
    {
        [TestMethod]
        public void CreatePdfFileTest()
        {
            var competitionResult = GetCompetitionResult();

            var pdfWriter = new DinkToPdfWriter("DinkToPdfResults");

            InvokeAction(pdfWriter.CreatePdfFile, competitionResult, "DinkToPdf egyetlen pdf fájl látrehozásának ideje");

        }

        [TestMethod]
        public void Create10000PdfFileTest()
        {
            var competitionResult = GetCompetitionResult();

            var pdfWriter = new DinkToPdfWriter("DinkToPdfResults");

            InvokeAction(pdfWriter.Create10000PdfFile, competitionResult, "DinkToPdf 10000 pdf fájl látrehozásának ideje");
        }
    }
}
