using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MissingTime : ControllerBase
    {
        private IMissingTimeService MissingTimeService { get; set; }
        public MissingTime(IMissingTimeService missingTimeService)
        {
            MissingTimeService = missingTimeService;
        }

        [HttpGet]
        public async Task<IEnumerable<Core.Model.MissingTime.MissingTime>> Get(DateTime startDate, DateTime endDate)
        {
            return await MissingTimeService.GetMissingTime(startDate, endDate);
        }
    }
}
