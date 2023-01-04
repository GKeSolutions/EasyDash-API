using MimeKit;

namespace Notification
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public List<MailboxAddress> Cc { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(IEnumerable<string> to, IEnumerable<string> cc, string subject, string content)
        {
            To = new List<MailboxAddress>();
            Cc = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x, x)));
            Cc.AddRange(cc.Select(x => new MailboxAddress(x, x)));
            Subject = subject;
            Content = content;
        }
    }
}
