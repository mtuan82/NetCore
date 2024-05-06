using Hangfire.Dashboard;
using System.Diagnostics.CodeAnalysis;

namespace HangfireJob.Utils
{
    public class HanfireDashBoardAuth : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
