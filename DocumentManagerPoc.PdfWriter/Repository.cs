using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DocumentManagerPoc.PdfWriter
{
    public interface IRepository
    {
        CompetitionResult GetCompetitionResult();
    }

    public class Repository : IRepository
    {
        public CompetitionResult GetCompetitionResult()
        {
            var year = 2016;
            var competition = "UEFA Champions League";

            using (var client = CreateClient())
            {
                var competitionResponse = client.GetAsync($"/api/football_matches?name={competition}&year={year}&page=1");
                var resultContent = competitionResponse.Result.Content.ReadAsStringAsync();
                var competitionResult = JsonConvert.DeserializeObject<CompetitionResult>(resultContent.Result);

                for (int i = competitionResult.page + 1; i <= competitionResult.total_pages; i++)
                {
                    var pageResponse = client.GetAsync($"/api/football_matches?name={competition}&year={year}&page={i}");

                    var pageResultContent = pageResponse.Result.Content.ReadAsStringAsync();

                    var pageResult = JsonConvert.DeserializeObject<CompetitionResult>(pageResultContent.Result);

                    competitionResult.data.AddRange(pageResult.data);
                }

                return competitionResult;
            }
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://jsonmock.hackerrank.com")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }


}
