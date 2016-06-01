using CommandLine;
using CommandLine.Text;

namespace VSTS.WIT.Reporting.Providers.JsonFile
{
    public class JsonFileTargetProviderOptions
    {
        [Option("targetPath", Required = true, HelpText = "Full path to where json files will be written to")]
        public string Path { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var helpText = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

            return helpText;
        }
    }
}
