using System;
using System.Threading.Tasks;
using CommandLine;
using VSTS.WIT.Reporting.Common;
using VSTS.WIT.Reporting.Providers;
using VSTS.WIT.Reporting.Providers.JsonFile;
using VSTS.WIT.Reporting.Providers.SQLite;
using VSTS.WIT.Reporting.Providers.VstsRest;

namespace VSTS.WIT.Reporting.Runtime
{
    public class ReportingRuntime
    {
        public static void Main(string[] args)
        {
            ISourceProvider sourceProvider = null;
            ITargetProvider targetProvider = null;
            
            var parser = new Parser(settings => 
            {
                settings.IgnoreUnknownArguments = true;
                settings.HelpWriter = Parser.Default.Settings.HelpWriter;
            });

            var options = new Options();
            if (parser.ParseArguments(args, options))
            {
                switch (options.SourceProvider)
                {
                    case "VstsRest":
                        {
                            var providerOptions = new VstsRestSourceProviderOptions();
                            if (parser.ParseArguments(args, providerOptions))
                            {
                                sourceProvider = new VstsRestSourceProvider(providerOptions);
                            }
                            break;
                        }
                    case "JsonFile":
                        {
                            var providerOptions = new JsonFileSourceProviderOptions();
                            if (parser.ParseArguments(args, providerOptions))
                            {
                                sourceProvider = new JsonFileSourceProvider(providerOptions);
                            }
                            else
                            {
                                
                            }
                            break;
                        }
                    default:
                        Console.Error.WriteLine("Invalid sourceProvider");
                        break;
                        
                }

                if (sourceProvider == null)
                {
                    return;
                }

                switch (options.TargetProvider)
                {
                    case "SQLite":
                        {
                            var providerOptions = new SQLiteTargetProviderOptions();
                            if (parser.ParseArguments(args, providerOptions))
                            {
                                targetProvider = new SQLiteTargetProvider(providerOptions);
                            }
                            break;
                        }
                    case "JsonFile":
                        {
                            var providerOptions = new JsonFileTargetProviderOptions();
                            if (parser.ParseArguments(args, providerOptions))
                            {
                                targetProvider = new JsonFileTargetProvider(providerOptions);
                            }
                            break;
                        }
                    default:
                        Console.Error.WriteLine("Invalid targetProvider");
                        break;
                }

                if (targetProvider == null)
                {
                    return;
                }
            }

            if (sourceProvider != null && targetProvider != null)
            {
                Task.Run(async () =>
                {
                    var runtime = new ReportingRuntime();
                    await runtime.Export(sourceProvider, targetProvider);
                }).Wait();
            }
        }

        public async Task Export(ISourceProvider source, ITargetProvider target)
        {
            do
            {
                var workItems = await source.GetWorkItems();
                await target.SaveWorkItems(workItems);
            }
            while (source.HasMoreWorkItems());
        }
    }
}
