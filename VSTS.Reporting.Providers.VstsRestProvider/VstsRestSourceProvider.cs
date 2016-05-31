using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using VSTS.Reporting.Common;

namespace VSTS.Reporting.Providers.VstsRestProvider
{
    public class VstsRestSourceProvider : ISourceProvider
    {
        private Configuration config;
        private Uri host;
        private VssCredentials credentials;
        private WorkItemTrackingHttpClient client;
        private ReportingWorkItemRevisionsFilter filter;
        private bool hasMoreWorkItems;

        public VstsRestSourceProvider(Configuration config)
        {
            this.config = config;
            this.host = new Uri((string)config["account"]);
            this.credentials = new VssBasicCredential((string)config["username"], (string)config["personalAccessToken"]);
            this.client = new WorkItemTrackingHttpClient(host, credentials);
        }

        public async Task<IEnumerable<WorkItem>> GetWorkItems()
        {
            var continuationToken = GetContinuationToken();
            var filter = await GetFilter();
            var batch = await RetryHelper.Retry(async () =>
            {
                return await client.ReadReportingRevisionsPostAsync(
                    filter, 
                    continuationToken: continuationToken);
            }, 5);

            this.hasMoreWorkItems = !batch.IsLastBatch;
            continuationToken = batch.ContinuationToken;
            SaveContinuationToken(continuationToken);

            return batch.Values;
        }

        public bool HasMoreWorkItems()
        {
            return hasMoreWorkItems;
        }

        private string GetContinuationToken()
        {
            if (config.ContainsKey("continuationToken"))
            {
                return (string)config["continuationToken"];
            }
            else
            {
                return null;
            }
        }

        private void SaveContinuationToken(string continuationToken)
        {
            config["continuationToken"] = continuationToken;
            config.Save();
        }

        private async Task<ReportingWorkItemRevisionsFilter> GetFilter()
        {
            if (filter == null)
            {
                filter = new ReportingWorkItemRevisionsFilter();
                filter.Fields = await GetAllFieldReferenceNames();
            }

            return filter;
        }

        private async Task<IEnumerable<string>> GetAllFieldReferenceNames()
        {
            var fields = await client.GetFieldsAsync();

            return fields.Select(f => f.ReferenceName);
        }
    }
}
