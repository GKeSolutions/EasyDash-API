﻿using Core.Enum;
using Core.Interface;
using Core.Model.Notification;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class Notify : BaseController
    {
        private readonly INotificationService NotificationService;
        private readonly IProcessService ProcessService;
        private readonly IMissingTimeService MissingTimeService;
        private readonly ILookupService LookupService;
        private readonly IConfiguration Configuration;

        public Notify(INotificationService notificationService, IProcessService processService, IMissingTimeService missingTimeService, ILookupService lookupService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration):base(lookupService, httpContextAccessor)
        {
            NotificationService = notificationService;
            ProcessService = processService;
            MissingTimeService = missingTimeService;
            LookupService = lookupService;
            Configuration = configuration;
        }

        [HttpPost]
        public async Task<bool> Process([FromBody] ProcessNotification processNotification)
        {
            if(processNotification.ProcessId != Guid.Empty)
            {
                var info = await ProcessService.GetProcessInfoByProcId(processNotification.ProcessId);
                if (info is null) throw new Exception("Process already complete");
                if (string.IsNullOrEmpty(info.UserEmail)) throw new Exception("Missing email address.");
                var tags = BuildProcessTags(info.UserName, info.ProcessCaption, info.LastUpdated, info.ProcessItemId);
                return await NotificationService.SendEmailNotification(new EmailNotification { EmailAddress = info.UserEmail, CcContact = processNotification.CcContact, EventType = (int)EventType.ActionList, ProcessCode = processNotification.ProcessCode, UserId = processNotification.UserId, ProcessDescription = info.ProcessCaption, ProcItemId = info.ProcessItemId, LastAccessTime = info.LastUpdated, TriggeredBy = await LookupService.GetUserIdByNetworkAlias(UserName), IsManual = true }, tags);
            }
            else if (processNotification.UserId != Guid.Empty)//User NotifyAll
            {
                var processes = await ProcessService.GetProcessesByUser(processNotification.UserId);
                foreach (var process in processes)
                {
                    if (process.Users != null || process?.Users?.Count != 0)
                    {
                        if (string.IsNullOrEmpty(process?.Users?.FirstOrDefault()?.UserEmail)) throw new Exception("Missing email address.");
                        else
                        {
                            var notification = new EmailNotification
                            {
                                EmailAddress = process?.Users?.FirstOrDefault()?.UserEmail,
                                CcContact = processNotification.CcContact,
                                ProcessCode = process?.ProcessCode,
                                EventType = (int)EventType.ActionList,
                                UserId = processNotification.UserId,
                                ProcessDescription = process.ProcessCaption,
                                ProcItemId = process.ProcessItemId,
                                LastAccessTime = process.LastUpdated,
                                TriggeredBy = await LookupService.GetUserIdByNetworkAlias(UserName),
                                IsManual=true
                            };
                            var tags = BuildProcessTags(process.UserName, process.ProcessCaption, process.LastUpdated, process.ProcessItemId);
                            await NotificationService.SendEmailNotification(notification, tags);
                        }
                    }
                }
            }
            else if(processNotification.ProcessCode != null) //process notify all
            {
                var atLeastOneEmailSent = false;
                //var processItems = await DashboardService.GetProcessItemsByProcessCode(processNotification.ProcessCode);
                var processItems = await ProcessService.GetProcessItemsByProcessCodeFromDb(processNotification.ProcessCode);
                foreach (var processItem in processItems)
                {
                    if (!string.IsNullOrEmpty(processItem.UserEmail)) //throw new Exception("User does not have valid email address.");
                    {
                        var notification = new EmailNotification
                        {
                            EmailAddress = processItem.UserEmail,
                            CcContact = processNotification.CcContact,
                            ProcessCode = processNotification.ProcessCode,
                            EventType = (int)EventType.ActionList,
                            UserId = processItem.UserId,
                            ProcessDescription = processItem.ProcessCaption,
                            ProcItemId = processItem.ProcessItemId,
                            LastAccessTime = processItem.LastUpdated,
                            TriggeredBy = await LookupService.GetUserIdByNetworkAlias(UserName),
                            IsManual = true
                        };
                        var tags = BuildProcessTags(processItem.UserName, processItem.ProcessCaption, processItem.LastUpdated, processItem.ProcessItemId);
                        await NotificationService.SendEmailNotification(notification, tags);
                        atLeastOneEmailSent = true;
                    }
                }
                if(!atLeastOneEmailSent) throw new Exception("Missing email address.");
            }
            return true;
        }

        [HttpPost]
        public async Task<bool> MissingTime([FromBody] MissingTimeNotification missingTimeNotification)
        {
            if (missingTimeNotification.IsOneWeek)
            {
                if (missingTimeNotification.IsOneUser)
                {
                    var missingTime = await MissingTimeService.GetTimePerUserPerWeek(missingTimeNotification.UserId, missingTimeNotification.StartDate, missingTimeNotification.EndDate);
                    if (string.IsNullOrEmpty(missingTime.EmailAddress)) throw new Exception("Missing email address.");
                    else if (missingTime.WorkHrs < missingTime.WeeklyHoursRequired)
                    {
                        var notification = new EmailNotification
                        {
                            EmailAddress = missingTime.EmailAddress,
                            CcContact = missingTimeNotification.CcContact,
                            EventType = (int)EventType.MissingTime,
                            UserId = missingTimeNotification.UserId,
                            RequiredHours=missingTime.WeeklyHoursRequired,
                            LoggedHours=missingTime.WorkHrs,
                            MissingHours= missingTime.WeeklyHoursRequired - missingTime.WorkHrs,
                            TriggeredBy = await LookupService.GetUserIdByNetworkAlias(UserName),
                            IsManual = true
                        };
                        var tags = BuildMissingTimeTags(missingTime.UserName, missingTimeNotification.StartDate, missingTime.WeeklyHoursRequired, missingTime.WorkHrs);
                        await NotificationService.SendEmailNotification(notification, tags);
                    }
                }
                else
                {
                    var atLeastOneEmailSent = false;
                    var missingTimeUsers = await MissingTimeService.GetUsersTimePerWeek(missingTimeNotification.StartDate, missingTimeNotification.EndDate);
                    foreach (var user in missingTimeUsers)
                    {
                        if (!string.IsNullOrEmpty(user.EmailAddress))
                        {
                            if (user.WorkHrs < user.WeeklyHoursRequired)
                            {
                                var notification = new EmailNotification
                                {
                                    EmailAddress = user.EmailAddress,
                                    CcContact = missingTimeNotification.CcContact,
                                    EventType = (int)EventType.MissingTime,
                                    UserId = user.UserId,
                                    RequiredHours = user.WeeklyHoursRequired,
                                    LoggedHours = user.WorkHrs,
                                    MissingHours = user.WeeklyHoursRequired - user.WorkHrs,
                                    TriggeredBy = await LookupService.GetUserIdByNetworkAlias(UserName),
                                    IsManual = true
                                };
                                var tags = BuildMissingTimeTags(user.UserName, missingTimeNotification.StartDate, user.WeeklyHoursRequired, user.WorkHrs);
                                await NotificationService.SendEmailNotification(notification, tags);
                                atLeastOneEmailSent |= true;
                            }
                        }
                    }
                    if (!atLeastOneEmailSent) throw new Exception("Missing email address.");
                }
            }
            else
            {
                var atLeastOneEmailSent = false;
                var weeks = await MissingTimeService.GetWeeksTimePerUser(missingTimeNotification.UserId, missingTimeNotification.StartDate, missingTimeNotification.EndDate);
                foreach (var week in weeks)
                {
                    if (!string.IsNullOrEmpty(week.EmailAddress))
                    {
                        if (week.WorkHrs < week.WeeklyHoursRequired)
                        {
                            var notification = new EmailNotification
                            {
                                EmailAddress = week.EmailAddress,
                                CcContact = missingTimeNotification.CcContact,
                                EventType = (int)EventType.MissingTime,
                                UserId = missingTimeNotification.UserId,
                                RequiredHours = week.WeeklyHoursRequired,
                                LoggedHours = week.WorkHrs,
                                MissingHours = week.WeeklyHoursRequired - week.WorkHrs,
                                TriggeredBy = await LookupService.GetUserIdByNetworkAlias(UserName),
                                IsManual = true
                            };
                            var tags = BuildMissingTimeTags(week.UserName, week.WeekStartDate, week.WeeklyHoursRequired, week.WorkHrs);
                            await NotificationService.SendEmailNotification(notification, tags);
                            atLeastOneEmailSent=true;
                        }
                    }   
                }
                if (!atLeastOneEmailSent) throw new Exception("Missing email address.");
            }
            return true;
        }

        private Dictionary<string, string> BuildProcessTags(string userName, string processCaption, string lastUpdated, Guid processItemId)
        {
            var tags = new Dictionary<string, string>();
            tags["UserName"] = userName;
            tags["ProcessCaption"] = processCaption;
            tags["LastUpdated"] = lastUpdated;
            tags["ProcessLink"] = "<a href=" + Configuration["InstanceConfiguration:BaseUrl"] + "Process/" + processItemId + "> Process Link</a> ";
            return tags;
        }

        private Dictionary<string, string> BuildMissingTimeTags(string userName, DateTime weekName, decimal requiredHours, int loggedHours)
        {
            var tags = new Dictionary<string, string>();
            tags["UserName"] = userName;
            tags["WeekName"] = weekName.ToString("MM/dd/yyyy");
            tags["MissingHours"] = (requiredHours - loggedHours).ToString();
            tags["RequiredHours"] = requiredHours.ToString();
            tags["LoggedHours"] = loggedHours.ToString();
            return tags;
        }
    }
}
