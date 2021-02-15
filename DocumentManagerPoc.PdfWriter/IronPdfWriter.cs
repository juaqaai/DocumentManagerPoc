using IronPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DocumentManagerPoc.PdfWriter
{
    public class IronPdfWriter : IPdfWriter
    {
        private static readonly string FileName = "uefa_champions_league";

        private readonly string relativePath;

        public IronPdfWriter(string relativePath)
        {
            this.relativePath = relativePath;
        }

        public void Create10000PdfFile(CompetitionResult competitionResult)
        {
            int pageNumber = 1;
            for (int i = 1; i <= 10000; i++)
            {
                if (pageNumber >=  competitionResult.total_pages)
                {
                    pageNumber = 1;
                }
                else 
                {
                    pageNumber++;
                }

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
            var fullPath = Path.GetFullPath(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."), relativeFilePath));

            var html = CreateHtml(matches);

            using (var renderer = new HtmlToPdf())
            {
                var pdfDocument = renderer.RenderHtmlAsPdf(html);

                pdfDocument.SaveAs(fullPath);
            }
        }

        private string CreateHtml(List<Match> matches)
        {
            var headerData = MatchHeader.Data.Split(';');

            return $@"<!DOCTYPE html>
                            <html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
                            <head>
                            <meta charset=""utf-8""/>
                            <title></title>
                            <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css"">
                            </head>
                        <body>
                            <div class=""container"">
                                <h1>UEFA champions league</h1>
                                <p>Mérkőzések</p>
                                <div class=""row"">
                                    <div class=""col-sm-1"" style=""background-color: #0099cc"">{headerData[0]}</div>
                                    <div class=""col-sm-3"" style=""background-color: #0099cc"">{headerData[1]}</div>
                                    <div class=""col-sm-3"" style=""background-color: #ffff99"">{headerData[2]}</div>
                                    <div class=""col-sm-3"" style=""background-color: #79d279"">{headerData[3]}</div>
                                    <div class=""col-sm-1"" style=""background-color: #ffff99"">{headerData[4]}</div>
                                    <div class=""col-sm-1"" style=""background-color: #79d279"">{headerData[5]}</div>
                                </div>
                                {CreateHtmlGrid(matches)}
                            </div>
                        </body>
                    </html>";
        }

        private string CreateHtmlGrid(List<Match> matches)
        {
            var sb = new StringBuilder();

            var count = 1;

            foreach (var match in matches)
            {
                sb.AppendLine($@"<div class=""row"">
                                    <div class=""col-sm-1"" style=""background-color: #0099cc"">{count}</div>
                                    <div class=""col-sm-3"" style=""background-color: #0099cc"">{match.round}</div>
                                    <div class=""col-sm-3"" style=""background-color: #ffff99"">{match.team1}</div>
                                    <div class=""col-sm-3"" style=""background-color: #79d279"">{match.team2}</div>
                                    <div class=""col-sm-1"" style=""background-color: #ffff99"">{match.team1goals}</div>
                                    <div class=""col-sm-1"" style=""background-color: #79d279"">{match.team2goals}</div>
                                </div>");
            }

            return sb.ToString();
        }
    }
}
