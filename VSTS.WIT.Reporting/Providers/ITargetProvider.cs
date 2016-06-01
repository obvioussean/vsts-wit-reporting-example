using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace VSTS.WIT.Reporting.Providers
{
    public interface ITargetProvider
    {
        Task SaveWorkItems(IEnumerable<WorkItem> workItems);
    }
}
