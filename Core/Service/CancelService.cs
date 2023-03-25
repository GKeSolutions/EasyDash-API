using Core.Interface;
using Core.Model.Dashboard;
using Core.Model.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Service
{
    public class CancelService : ICancelService
    {
        private IProcessService ProcessService;
        private ILookupService LookupService;
        private INotificationService NotificationService;
        private IConfiguration Configuration;
        private ILogger Logger;
        private IHttpContextAccessor HttpContextAccessor;
        private string UserName;

        public CancelService(INotificationService notificationService, ILookupService lookupService, IProcessService processService, IConfiguration configuration, ILogger<ProcessService> logger, IHttpContextAccessor httpContextAccessor)
        {
            ProcessService = processService;
            LookupService = lookupService;
            NotificationService = notificationService;
            Configuration = configuration;
            Logger = logger;
            HttpContextAccessor = httpContextAccessor;
            UserName = HttpContextAccessor?.HttpContext?.User?.Identity?.Name;
        }

        public async Task<string> CancelProcess(string processCode, Guid procItemId)
        {
            Logger.LogInformation($"{UserName} - {nameof(CancelService)} - {nameof(CancelProcess)} {processCode} {procItemId}");
            var builder = new TransactionServiceBuilder(Configuration);
            var info = await ProcessService.GetProcessInfoByProcId(procItemId);
            if (info is null) throw new System.Exception("Process already complete");
            var messageHistory = new MessageHistory
            {
                EventType = 1,
                IsManual = true,
                IsReassign = true,
                IsCancelProcess = true,
                IsSystem = false,
                ProcessCode = processCode,
                ProcessDescription = info.ProcessDescription,
                ProcItemId = procItemId,
                LastAccessTime = info.LastUpdated,
                TriggeredBy = await LookupService.GetUserIdByNetworkAlias(UserName),
                UserId = info.UserId
            };
            await NotificationService.AddNotificationHistory(messageHistory);
            return await builder.CancelProcess(processCode, procItemId);
        }

        public async Task<string> CancelAll(Cancel model)
        {
            Logger.LogInformation($"{UserName} - {nameof(CancelService)} - {nameof(CancelAll)} {model.ProcessCode} {model.ProcItemId}");
            if (model.ProcessCode != null)
            {
                var processItems = await ProcessService.GetProcessItemsByProcessCodeFromDb(model.ProcessCode);
                foreach (var processItem in processItems)
                {
                    await CancelProcess(model.ProcessCode, processItem.ProcessItemId);
                }
            }

            else if (model.UserId != Guid.Empty)
            {
                var processes = await ProcessService.GetProcessesByUserFromDb(model.UserId);
                foreach (var process in processes)
                {
                    await CancelProcess(process.ProcessCode, process.ProcessItemId);
                }
            }
            return string.Empty;
        }
    }
}
