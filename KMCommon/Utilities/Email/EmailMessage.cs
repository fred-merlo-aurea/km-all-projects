using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace KM.Common.Utilities.Email
{
    public class EmailMessage
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public string From { get; set; }

        public IList<string> To { get; private set; }

        public IList<string> CarbonCopy { get; private set; }

        public IList<Attachment> Attachments { get; private set; }

        public IList<AlternateView> AlternateViews { get; private set; }

        public bool IsHtml { get; set; }

        public MailPriority Priority { get; set; }

        public EmailMessage()
        {
            Subject = String.Empty;
            Body = String.Empty;
            From = String.Empty;
            IsHtml = true;
            Priority = MailPriority.High;
            To = new List<string>();
            CarbonCopy = new List<string>();
            Attachments = new List<Attachment>();
            AlternateViews = new List<AlternateView>();
        }

        public void AddRange<T>(IList<T> source, IList<T> range) where T : class
        {
            Guard.NotNull(source, nameof(source));

            if (range == null)
            {
                return;
            }

            foreach (var item in range)
            {
                if (item == null)
                {
                    continue;
                }

                source.Add(item);
            }
        }
    }
}