using CommandLine;
using CommandLine.Text;

namespace VSTS.WIT.Reporting.Providers.VstsRest
{
    public class VstsRestSourceProviderOptions
    {
        [Option("account", Required = true, HelpText = "Account to use for reading work items")]
        public string Account { get; set; }

        [Option("username", Required = true, HelpText = "Username to authenticate with")]
        public string Username { get; set; }

        [Option("personalAccessToken", Required = true, HelpText = "Personal access token to authenticate with")]
        public string PersonalAccessToken { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var helpText = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

            return helpText;
        }
    }
}
