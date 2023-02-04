using Core.Interface;
using Hangfire;

namespace Core.Service
{
    public class JobService: IJobService
    {
        public async Task<bool> AddJob(string jobDescription, string cronExpression)
        {
            RecurringJob.AddOrUpdate(jobDescription, () => Console.Write("Method to call should go here!"), cronExpression);
            return true;
        }
    }
}
