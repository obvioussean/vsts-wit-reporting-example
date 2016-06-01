using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using VSTS.WIT.Reporting.Providers.JsonFile;

namespace VSTS.WIT.Reporting.Common
{
    public class Options
    {
        [Option("sourceProvider", Required = true, HelpText = "Source for work items.  Possible options are VstsRest or JsonFile")]
        public string SourceProvider { get; set; }

        [Option("targetProvider", Required = true, HelpText = "Destination for work items.  Possible options are JsonFile or SQLite")]
        public string TargetProvider { get; set; }

        [ValueList(typeof(List<string>))]
        public IList<string> ProviderOptions { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var helpText = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

            return helpText;
        }
    }
}
