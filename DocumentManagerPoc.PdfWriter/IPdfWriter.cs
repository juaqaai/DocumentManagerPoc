namespace DocumentManagerPoc.PdfWriter
{
    public interface IPdfWriter
    {
        void Create10000PdfFile(CompetitionResult competitionResult);

        void CreatePdfFile(CompetitionResult competitionResult);
    }
}
