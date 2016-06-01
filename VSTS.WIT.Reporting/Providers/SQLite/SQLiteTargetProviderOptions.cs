using CommandLine;
using CommandLine.Text;

namespace VSTS.WIT.Reporting.Providers.SQLite
{
    public class SQLiteTargetProviderOptions
    {
        [Option("databaseFile", Required = true, HelpText = "Location of the SQLite database file")]
        public string DatabaseFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var helpText = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

            return helpText;
        }
    }
}
