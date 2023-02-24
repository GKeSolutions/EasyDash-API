using MimeKit;

namespace Notification
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public List<MailboxAddress>? Cc { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(IEnumerable<string> to, IEnumerable<string> cc, string subject, string content, bool isTestMode, IEnumerable<string> emailsForTestMode)
        {
            To = new List<MailboxAddress>();
            if (isTestMode) To.AddRange(emailsForTestMode.Select(x => new MailboxAddress(x, x)));
            else {
                To.AddRange(to.Select(x => new MailboxAddress(x, x)));
                if (cc is null) Cc = null;
                else
                {
                    Cc = new List<MailboxAddress>();
                    Cc.AddRange(cc.Select(x => new MailboxAddress(x, x)));
                }
            } 
            
            Subject = subject;
            Content = content;

        }
    }
}
