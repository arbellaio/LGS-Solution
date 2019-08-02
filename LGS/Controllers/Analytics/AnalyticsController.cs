using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KruisIT.Web.Analytics.Controllers;
using LGS.AppProperties;

namespace LGS.Controllers.Analytics
{
    public class AnalyticsController : KruisIT.Web.Analytics.Controllers.ReportController {
        public AnalyticsController() : base(AppConstants.ConnectionName)
        {
        }

    }
}