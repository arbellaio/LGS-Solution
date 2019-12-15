using LGS.AppProperties;

namespace LGS.Controllers.Analytics
{
    public class AnalyticsController : KruisIT.Web.Analytics.Controllers.ReportController {
        public AnalyticsController() : base(AppConstants.ConnectionName)
        {
        }

    }
}