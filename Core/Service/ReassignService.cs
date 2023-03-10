using Core.Interface;
using Core.Model.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Service
{
    public class ReassignService : IReassignService
    {
        private IConfiguration Configuration;
        INotificationService NotificationService;
        IDashboardService DashboardService;
        ILookupService LookupService;
        private readonly ILogger<ReassignService> Logger;
        private IHttpContextAccessor HttpContextAccessor;
        private string UserName;

        public ReassignService(IConfiguration configuration, INotificationService notificationService, IDashboardService dashboardService, ILookupService lookupService, ILogger<ReassignService> logger, IHttpContextAccessor httpContextAccessor)
        {
            Configuration= configuration;
            NotificationService= notificationService;
            DashboardService= dashboardService;
            LookupService= lookupService;
            Logger= logger;
            HttpContextAccessor= httpContextAccessor;
            UserName = HttpContextAccessor.HttpContext.User.Identity.Name;
        }

        public async Task<string> Reassign(string processCode, Guid procItemId, Guid reassignToUserId, string userName)
        {
            Logger.LogInformation($"{UserName} - {nameof(ReassignService)} - {nameof(Reassign)} {processCode} {procItemId} {reassignToUserId}");
            var builder = new TransactionServiceBuilder(Configuration);
            var info = await DashboardService.GetProcessInfoByProcId(procItemId);
            var messageHistory = new MessageHistory
            {
                EventType = 1,
                IsManual = true,
                IsReassign = true,
                IsSystem = false,
                ReassignTo = reassignToUserId,
                ProcessCode = processCode,
                ProcessDescription = info.ProcessDescription,
                ProcItemId = procItemId,
                LastAccessTime = info.LastUpdated,
                TriggeredBy = await LookupService.GetUserIdByNetworkAlias(userName),
                UserId = info.UserId
            };
            await NotificationService.AddNotificationHistory(messageHistory);
            return await builder.Reassign(processCode, procItemId, reassignToUserId);
        }
    }
}
