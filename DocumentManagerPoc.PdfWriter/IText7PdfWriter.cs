using iText.IO.Font.Constants;
using iText.IO.Util;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using Path = System.IO.Path;

namespace DocumentManagerPoc.PdfWriter
{
    public class IText7PdfWriter : IPdfWriter
    {
        private static readonly string FileName = "uefa_champions_league";

        private static readonly Color GreenColor = new DeviceCmyk(0.78f, 0, 0.81f, 0.21f);
        private static readonly Color YellowColor = new DeviceCmyk(0, 0, 0.76f, 0.01f);
        private static readonly Color RedColor = new DeviceCmyk(0, 0.76f, 0.86f, 0.01f);
        private static readonly Color BlueColor = new DeviceCmyk(0.28f, 0.11f, 0, 0);

        private readonly string relativePath;

        public IText7PdfWriter(string relativePath)
        {
            this.relativePath = relativePath;
        }

        public void Create10000PdfFile(CompetitionResult competitionResult)
        {
            for (int i = 1; i <= 10000; i++)
            {
                var pageNumber = i >= competitionResult.total ? competitionResult.page : competitionResult.page;

                var matches = competitionResult.data.Skip(competitionResult.per_page * (pageNumber - 1))
                                                    .Take(competitionResult.per_page).ToList();

                CreatePdfFile(matches, Path.Combine(relativePath, $"{FileName}_{i}.pdf"));
            }
        }

        public void CreatePdfFile(CompetitionResult competitionResult)
        {
            CreatePdfFile(competitionResult.data, Path.Combine(relativePath, $"{FileName}.pdf"));
        }

        private void CreatePdfFile(List<Match> matches, string relativeFilePath)
        {
            // bin\debug
            var fullPath = Path.GetFullPath(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."), relativeFilePath));

            var pdf = new PdfDocument(new iText.Kernel.Pdf.PdfWriter(fullPath));

            var ps = new PageSize(842, 680);

            var document = new Document(pdf, ps);

            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            var table = new Table(UnitValue.CreatePercentArray(new float[] { 1.5f, 7, 7, 7, 4, 4 }));

            table.SetTextAlignment(TextAlignment.CENTER)
                 .SetHorizontalAlignment(HorizontalAlignment.CENTER);

            ProcessHeader(table, MatchHeader.Data);

            var count = 1;

            foreach (var match in matches)
            {
                Process(table, match, font, count++);
            }

            document.Add(table);

            document.Close();
        }

        private void ProcessHeader(Table table, string line)
        {
            var tokenizer = new StringTokenizer(line, ";");

            while (tokenizer.HasMoreTokens())
            {
                var cell = new Cell().Add(new Paragraph(tokenizer.NextToken()));
                cell.SetNextRenderer(new RoundedCornersCellRenderer(cell));
                cell.SetPadding(5).SetBorder(null);
                table.AddHeaderCell(cell);
            }
        }

        private void Process(Table table, Match match, PdfFont font, int count)
        {
            table.AddCell(CreateCell(count.ToString(), font, BlueColor));

            table.AddCell(CreateCell(match.round, font, BlueColor));

            table.AddCell(CreateCell(match.team1, font, GreenColor));

            table.AddCell(CreateCell(match.team2, font, YellowColor));

            table.AddCell(CreateCell(match.team1goals.ToString(), font, GreenColor));

            table.AddCell(CreateCell(match.team2goals.ToString(), font, YellowColor));
        }

        private Cell CreateCell(string data, PdfFont font, Color backgroundColor)
        {
            Cell cell = new Cell().Add(new Paragraph(data));
            cell.SetFont(font).SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f));
            cell.SetBackgroundColor(backgroundColor);
            return cell;
        }

        private class RoundedCornersCellRenderer : CellRenderer
        {

            public RoundedCornersCellRenderer(Cell modelElement) : base(modelElement)
            {
            }

            public override void DrawBorder(DrawContext drawContext)
            {
                Rectangle rectangle = GetOccupiedAreaBBox();
                float llx = rectangle.GetX() + 1;
                float lly = rectangle.GetY() + 1;
                float urx = rectangle.GetX() + GetOccupiedAreaBBox().GetWidth() - 1;
                float ury = rectangle.GetY() + GetOccupiedAreaBBox().GetHeight() - 1;
                PdfCanvas canvas = drawContext.GetCanvas();
                float r = 4;
                float b = 0.4477f;
                canvas.MoveTo(llx, lly).LineTo(urx, lly).LineTo(urx, ury - r)
                        .CurveTo(urx, ury - r * b, urx - r * b, ury, urx - r, ury)
                        .LineTo(llx + r, ury)
                        .CurveTo(llx + r * b, ury, llx, ury - r * b, llx, ury - r)
                        .LineTo(llx, lly).Stroke();

                base.DrawBorder(drawContext);
            }
        }
    }
}
