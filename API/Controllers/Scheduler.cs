﻿using Core.Interface;
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
        }

        [HttpPut]
        public async Task<SchedulerModel> UpdateScheduler([FromBody] SchedulerModel scheduler)
        {
            return await SchedulerService.UpdateScheduler(scheduler);
        }

        [HttpDelete]
        public async Task<int> DeleteScheduler(int scheduler)
        {
            return await SchedulerService.DeleteScheduler(scheduler);
        }
    }
}