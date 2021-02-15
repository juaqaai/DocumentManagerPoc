using DocumentManagerPoc.PdfWriter;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;

namespace DocumentManagerPoc.PdfWriterTest
{
    public class PdfWriterTestBase
    {
        private CompetitionResult _competitionResult;

        public PdfWriterTestBase()
        {
            var path = Path.GetFullPath(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."), "Logs/log.txt"));

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, retainedFileCountLimit: null)
                .CreateLogger();
        }

        protected void InvokeAction(Action<CompetitionResult> action, CompetitionResult competitionResult, string logMessage)
        {
            var watch = Stopwatch.StartNew();

            action(competitionResult);

            watch.Stop();

            WriteToLog(logMessage, watch.Elapsed);
        }

        protected CompetitionResult GetCompetitionResult()
        {
            if (_competitionResult == null)
            {
                var repository = new Repository();
                _competitionResult = repository.GetCompetitionResult();
            }
            return _competitionResult;
        }

        private void WriteToLog(string logMessage, TimeSpan elapsed)
        {
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        elapsed.Hours, elapsed.Minutes, elapsed.Seconds,
                        elapsed.Milliseconds / 10);

            Log.Information($"{logMessage}: {elapsedTime}");
        }
    }
}
