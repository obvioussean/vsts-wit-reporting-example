using CommandLine;
using CommandLine.Text;

namespace VSTS.WIT.Reporting.Providers.JsonFile
{
    public class JsonFileSourceProviderOptions
    {
        [Option("sourcePath", Required = true, HelpText = "Full path to where json files will be read from")]
        public string Path { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var helpText = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

            return helpText;
        }
    }
}
