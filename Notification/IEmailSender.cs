namespace Notification
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
