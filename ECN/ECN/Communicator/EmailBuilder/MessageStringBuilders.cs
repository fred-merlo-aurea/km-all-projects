using System.Text;

namespace ECN.Communicator.EmailBuilder
{
    public class MessageStringBuilders
    {
        public StringBuilder HtmlBody { get; } = new StringBuilder();
        public StringBuilder TextBody { get; } = new StringBuilder();
        public StringBuilder DynamicSubject { get; } = new StringBuilder();
        public StringBuilder DynamicFromEmail { get; } = new StringBuilder();
        public StringBuilder DynamicFromName { get; } = new StringBuilder();
        public StringBuilder DynamicReplyTo { get; } = new StringBuilder();
    }
}
