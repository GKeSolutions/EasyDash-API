using Core.Interface;
using Core.Model.Analytics;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[Action]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class Analytics : ControllerBase
    {
        public IAnalyticsService AnalyticsService { get; set; }

        public Analytics(IAnalyticsService analyticsService)
        {
            AnalyticsService = analyticsService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserAnalytic>> user(DateTime startDate, DateTime endDate)
        {
            return await AnalyticsService.GetAnalyticUsers(startDate, endDate);
        }

        [HttpGet]
        public async Task<IEnumerable<ProcessAnalytic>> process(DateTime startDate, DateTime endDate)
        {
            return await AnalyticsService.GetAnalyticProcesses(startDate, endDate);
        }

        [HttpGet]
        public async Task<IEnumerable<User>> UserList()
        {
            return await AnalyticsService.GetAnalyticUserList();
        }

        [HttpGet]
        public async Task<IEnumerable<Process>> ProcessList()
        {
            return await AnalyticsService.GetAnalyticProcessList();
        }
    }
}
