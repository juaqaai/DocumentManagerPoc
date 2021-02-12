using DocumentManagerPoc.PdfWriter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentManagerPoc.PdfWriterTest
{
    [TestClass]
    public class IronPdfWriterTest : PdfWriterTestBase
    {
        [TestMethod]
        public void CreatePdfFileTest()
        {
            var competitionResult = GetCompetitionResult();

            var pdfWriter = new IronPdfWriter("IronResults");

            InvokeAction(pdfWriter.CreatePdfFile, competitionResult, "IronPdf egyetlen pdf fájl látrehozásának ideje");

        }

        [TestMethod]
        public void Create10000PdfFileTest()
        {
            var competitionResult = GetCompetitionResult();

            var pdfWriter = new IronPdfWriter("IronResults");

            InvokeAction(pdfWriter.Create10000PdfFile, competitionResult, "IronPdf 10000 pdf fájl látrehozásának ideje");
        }
    }
}
