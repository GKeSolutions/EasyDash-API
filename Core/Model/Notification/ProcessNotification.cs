﻿namespace Core.Model.Notification
{
    public class ProcessNotification
    {
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string CcContact { get; set; }
        public string ProcessCode { get; set; }
        public Guid ProcessId { get; set; }
    }
}
