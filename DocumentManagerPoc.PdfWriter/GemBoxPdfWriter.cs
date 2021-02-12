using GemBox.Document;
using GemBox.Document.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DocumentManagerPoc.PdfWriter
{
    public class GemBoxPdfWriter : IPdfWriter
    {
        private static readonly string FileName = "uefa_champions_league";

        private readonly string relativePath;

        public GemBoxPdfWriter(string relativePath)
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
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            var fullPath = Path.GetFullPath(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."), relativeFilePath));

            var document = new DocumentModel();

            var section = new Section(document);
            document.Sections.Add(section);

            section.Blocks.Add(new Paragraph(document,
                                             new Run(document, "UEFA champions league") { CharacterFormat = { Size = 36 } },
                                             new SpecialCharacter(document, SpecialCharacterType.LineBreak),
                                             new Run(document, "Mérkőzések") { CharacterFormat = { Size = 14 } })
            );

            var table = new Table(document);
            table.TableFormat.PreferredWidth = new TableWidth(95, TableWidthUnit.Percentage);
            table.TableFormat.AutomaticallyResizeToFitContents = false;
            table.TableFormat.Style.TableFormat.Borders.ClearBorders();

            table.Columns.Add(new TableColumn(20));
            table.Columns.Add(new TableColumn(60));
            table.Columns.Add(new TableColumn(100));
            table.Columns.Add(new TableColumn(100));
            table.Columns.Add(new TableColumn(40));
            table.Columns.Add(new TableColumn(40));

            var rowCount = 0;
            for (; rowCount < matches.Count; rowCount++)
            {
                table.Rows.Add(new TableRow(document) { RowFormat = { Height = new TableRowHeight(30, TableRowHeightRule.AtLeast) } });
            }

            for (var r = 0; r < matches.Count; r++)
            {
                var match = matches[r];

                for (var c = 0; c < 6; c++)
                {
                    var paragraph = new Paragraph(document, GetParagraphText(match, r + 1, c))
                    {
                        ParagraphFormat = new ParagraphFormat
                        {
                            Alignment = HorizontalAlignment.Left,
                            KeepWithNext = true
                        }
                    };

                    var cell = new TableCell(document, paragraph)
                    {
                        CellFormat = new TableCellFormat
                        {
                            VerticalAlignment = VerticalAlignment.Bottom,
                            BackgroundColor = CreateColor(c)
                        }
                    };

                    table.Rows[r].Cells.Add(cell);
                }
            }

            document.Sections.Add(new Section(document, table));

            document.Save(fullPath, SaveOptions.PdfDefault);
        }

        private string GetParagraphText(Match match, int row, int column)
        {
            var header = MatchHeader.Data.Split(';');

            if (column == 0)
                return (row == 0) ? header[column] : row.ToString();

            if (column == 1)
                return (row == 0) ? header[column] : match.round;

            if (column == 2)
                return (row == 0) ? header[column] : match.team1;

            if (column == 3)
                return (row == 0) ? header[column] : match.team2;

            if (column == 4)
                return (row == 0) ? header[column] : match.team1goals.ToString();

            if (column == 5)
                return (row == 0) ? header[column] : match.team2goals.ToString();

            return null;
        }

        private Color CreateColor(int column)
        {
            if (column == 0 || column == 1)
            {
                return new Color(0, 153, 204);
            }
            else if (column == 2 || column == 4)
            {
                return new Color(255, 255, 153);
            }
            else
            {
                return new Color(121, 210, 121);
            }
        }
    }
}
