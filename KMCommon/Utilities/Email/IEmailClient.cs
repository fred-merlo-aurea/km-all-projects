using System.Net.Mail;

namespace KM.Common.Utilities.Email
{
    public interface IEmailClient
    {
        void Send(MailMessage message, string mailServer = null);

        bool SendAndSave(MailMessage message, string filePath, string mailServer = null);

        void SendUsingConfiguration(MailMessage message, string filePath, string mailServer = null);

        void Save(MailMessage message, string filePath, string mailServer = null);
    }
}