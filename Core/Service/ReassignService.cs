using Core.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Service
{
    public class ReassignService : IReassignService
    {
        private IConfiguration Configuration;
        private readonly ILogger<ReassignService> Logger;

        public ReassignService(IConfiguration configuration, ILogger<ReassignService> logger)
        {
            Configuration= configuration;
            Logger= logger;
        }

        public async Task<string> Reassign(string processCode, Guid procItemId, Guid reassignToUserId)
        {
            Logger.LogInformation($"{nameof(ReassignService)} - {nameof(Reassign)} {processCode} {procItemId} {reassignToUserId}");
            var builder = new TransactionServiceBuilder(Configuration);
            return await builder.Reassign(processCode, procItemId, reassignToUserId);
        }
    }
}
