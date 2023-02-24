using Core.Interface;
using Microsoft.Extensions.Configuration;

namespace Core.Service
{
    public class ReassignService : IReassignService
    {
        private IConfiguration Configuration;

        public ReassignService(IConfiguration configuration)
        {
            Configuration= configuration;
        }

        public async Task<string> Reassign(string processCode, Guid procItemId, Guid reassignToUserId)
        {
            var builder = new TransactionServiceBuilder(Configuration);
            return await builder.Reassign(processCode, procItemId, reassignToUserId);
        }
    }
}
