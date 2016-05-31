using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Newtonsoft.Json;
using VSTS.Reporting.Common;

namespace VSTS.Reporting.Providers.JsonFileProvider
{
    public class JsonFileTargetProvider : ITargetProvider
    {
        private string path;

        public JsonFileTargetProvider(Configuration config)
        {
            this.path = (string)config["jsonPath"];
            Directory.CreateDirectory(this.path);
        }

        public async Task SaveWorkItems(IEnumerable<WorkItem> workItems)
        {
            foreach (var workItem in workItems)
            {
                using (FileStream stream = new FileStream($@"{path}\{workItem.Id}-{workItem.Rev}.json", FileMode.Create))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    await writer.WriteAsync(JsonConvert.SerializeObject(workItem));
                }
            }
        }
    }
}
