using DocumentManagerPoc.PdfWriter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentManagerPoc.PdfWriterTest
{
    [TestClass]
    public class IText7PdfWriterTest : PdfWriterTestBase
    {
        [TestMethod]
        public void CreatePdfFileTest()
        {
            var competitionResult = GetCompetitionResult();

            var pdfWriter = new IText7PdfWriter("IText7Results");

            InvokeAction(pdfWriter.CreatePdfFile, competitionResult, "IText7 egyetlen pdf fájl látrehozásának ideje");
        }

        [TestMethod]
        public void Create10000PdfFileTest()
        {
            var competitionResult = GetCompetitionResult();

            var pdfWriter = new IText7PdfWriter("IText7Results");

            InvokeAction(pdfWriter.Create10000PdfFile, competitionResult, "IText7 10000 pdf fájl látrehozásának ideje");
        }


    }
}
