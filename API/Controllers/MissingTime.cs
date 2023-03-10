using Core.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class MissingTime : BaseController
    {
        private IMissingTimeService MissingTimeService { get; set; }
        public MissingTime(IMissingTimeService missingTimeService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
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
