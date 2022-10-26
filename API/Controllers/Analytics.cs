using Core.Interface;
using Core.Model.Analytics;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[Action]")]
    public class Analytics : ControllerBase
    {
        public IAnalyticsService AnalyticsService { get; set; }

        public Analytics(IAnalyticsService analyticsService)
        {
            AnalyticsService = analyticsService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserAnalytic>> user()
        {
            return await AnalyticsService.GetAnalytics();
        }
    }
}
