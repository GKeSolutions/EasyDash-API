using Core.Interface;
using Core.Model.Notification;
using Core.Model.Reassign;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Service
{
    public class ReassignService : IReassignService
    {
        private IConfiguration Configuration;
        INotificationService NotificationService;
        IProcessService ProcessService;
        ILookupService LookupService;
        private readonly ILogger<ReassignService> Logger;
        private IHttpContextAccessor HttpContextAccessor;
        private string UserName;

        public ReassignService(IConfiguration configuration, INotificationService notificationService, IProcessService processService, ILookupService lookupService, ILogger<ReassignService> logger, IHttpContextAccessor httpContextAccessor)
        {
            Configuration= configuration;
            NotificationService= notificationService;
            ProcessService = processService;
            LookupService= lookupService;
            Logger= logger;
            HttpContextAccessor= httpContextAccessor;
            UserName = HttpContextAccessor?.HttpContext?.User?.Identity?.Name;
        }

        public async Task<string> Reassign(string processCode, Guid procItemId, Guid reassignToUserId, bool isSystem=false)
        {
            Logger.LogInformation($"{UserName} - {nameof(ReassignService)} - {nameof(Reassign)} {processCode} {procItemId} {reassignToUserId}");
            var builder = new TransactionServiceBuilder(Configuration);
            var info = await ProcessService.GetProcessInfoByProcId(procItemId);
            if (info is null) throw new System.Exception("Process already complete");
            var messageHistory = new MessageHistory
            {
                EventType = 1,
                IsManual = true,
                IsReassign = true,
                IsSystem = isSystem,
                ReassignTo = reassignToUserId,
                ProcessCode = processCode,
                ProcessDescription = info.ProcessDescription,
                ProcItemId = procItemId,
                LastAccessTime = info.LastUpdated,
                TriggeredBy = isSystem ? null : await LookupService.GetUserIdByNetworkAlias(UserName),
                UserId = info.UserId
            };
            await NotificationService.AddNotificationHistory(messageHistory);
            return await builder.Reassign(processCode, procItemId, reassignToUserId);
        }

        public async Task<string> ReassignAll(ReassignModel model)
        {
            if (model.ProcessCode != null) 
            {
                var processItems = await ProcessService.GetProcessItemsByProcessCodeFromDb(model.ProcessCode);
                foreach ( var processItem in processItems ) 
                {
                    await Reassign(model.ProcessCode, processItem.ProcessItemId, model.ReassignToUserId);
                }
            }

            if (model.InitialUserId != Guid.Empty)
            {
                var processes = await ProcessService.GetProcessesByUserFromDb(model.InitialUserId);
                foreach (var process in processes)
                {
                    await Reassign(process.ProcessCode, process.ProcessItemId, model.ReassignToUserId);
                }
            }
            return string.Empty;
        }
    }
}
