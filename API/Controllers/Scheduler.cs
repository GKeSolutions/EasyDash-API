using Core.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SchedulerModel = Core.Model.Notification.Scheduler;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Scheduler : ControllerBase
    {
        private ISchedulerService SchedulerService { get; set; }
        public Scheduler(ISchedulerService schedulerService)
        {
            SchedulerService = schedulerService;
        }

        [HttpGet]
        public async Task<IEnumerable<SchedulerModel>> GetScheduler()
        {
            return await SchedulerService.GetScheduler();
        }

        [HttpPost]
        public async Task<SchedulerModel> CreateScheduler([FromBody] SchedulerModel scheduler)
        {
            return await SchedulerService.CreateScheduler(scheduler);
            //Add Hangfire Job?
        }

        [HttpPut]
        public async Task<SchedulerModel> UpdateScheduler([FromBody] SchedulerModel scheduler)
        {
            return await SchedulerService.UpdateScheduler(scheduler);
            //Check if it is linked to a ScheduledNotification
            //if yes, delete and recreate the job
            //Update Hangfire Job?
        }

        [HttpDelete]
        public async Task<int> DeleteScheduler(int scheduler)
        {
            return await SchedulerService.DeleteScheduler(scheduler);
            //Should be able to delete if it has an active scheduled notification?
            //Add Hangfire Job?
        }
    }
}
