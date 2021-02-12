using System.Collections.Generic;

namespace DocumentManagerPoc.PdfWriter
{
    public class CompetitionResult
    {
        public int page { get; set; }

        public int per_page { get; set; }

        public int total { get; set; }

        public int total_pages { get; set; }

        public List<Match> data { get; set; }
    }

    public class Match
    {
        public string round { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public int team1goals { get; set; }
        public int team2goals { get; set; }
    }

    public class MatchHeader
    {
        public const string Data = "Sorszám;Forduló;Hazai csapat;Vendég csapat;Hazai gólok;Vendég gólok";
    }
}
