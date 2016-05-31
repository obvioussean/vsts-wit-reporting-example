using System.Threading.Tasks;
using VSTS.Reporting.Providers;

namespace VSTS.Reporting.Runtime
{
    public class ReportingRuntime
    {
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
