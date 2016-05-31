var edge = require('edge');
var fs = require('fs');
var program = require('commander');
var path = require('path');

if (!fs.existsSync('./config.json')) {
    console.error("Plesae configure before running");
    process.exit(1);
}

program
    .version('0.0.1')
    .option('--source <source>', 'Source provider to run [VSTS, JSON]')
    .option('--target <target>', 'Target provider to run [JSON ,SQLite]')
    .parse(process.argv);

var sourceProvider;
switch (program.source) {
    case "VSTS":
        sourceProvider = "VstsRestSourceProvider";
        break;
    case "JSON":
        sourceProvider = "JsonFileSourceProvider";
        break;
    default:
        console.error("Invalid source provider specified.");
        process.exit(1);
}

var targetProvider;
switch (program.target) {
    case "JSON":
        targetProvider = "JsonFileTargetProvider";
        break;
    case "SQLite":
        targetProvider = "SQLiteTargetProvider";
        break;
    default:
        console.error("Invalid target provider specified.");
        process.exit(1);
}

var body = `
        #r "VSTS.Reporting.Common\\bin\\Debug\\VSTS.Reporting.Common.dll"
        #r "VSTS.Reporting.Runtime\\bin\\Debug\\VSTS.Reporting.Runtime.dll"
        #r "VSTS.Reporting.Providers\\bin\\Debug\\VSTS.Reporting.Providers.dll"
        #r "VSTS.Reporting.Providers.VstsRestProvider\\bin\\Debug\\VSTS.Reporting.Providers.VstsRestProvider.dll"
        #r "VSTS.Reporting.Providers.SQLiteProvider\\bin\\Debug\\VSTS.Reporting.Providers.SQLiteProvider.dll"
        #r "VSTS.Reporting.Providers.JsonFileProvider\\bin\\Debug\\VSTS.Reporting.Providers.JsonFileProvider.dll"
 
        using System.Threading.Tasks;
        using VSTS.Reporting.Common;
        using VSTS.Reporting.Runtime;
        using VSTS.Reporting.Providers.JsonFileProvider;
        using VSTS.Reporting.Providers.SQLiteProvider;
        using VSTS.Reporting.Providers.VstsRestProvider;
        
        public class Startup
        {
            public async Task<object> Invoke(object input)
            {
                var config = new Configuration(@"${path.join(__dirname, 'config.json')}");
            
                var reportingRuntime = new ReportingRuntime(); 
                var sourceProvider = new ${sourceProvider}(config);
                var targetProvider = new ${targetProvider}(config);
                
                await reportingRuntime.Export(sourceProvider, targetProvider);
                
                return "Complete";
            }
        }
    `;

fs.writeFileSync('./export.csx', body);

var exportFunction = edge.func(path.join(__dirname, 'export.csx'));
exportFunction(null, function (error, result) {
    if (error) {
        console.error(error);
    }
    else {
        console.log(result);
    }
});