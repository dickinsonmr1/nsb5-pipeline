using NServiceBus;
using NServiceBus.Logging;
using UserService.Messages.Events;
using WelcomeEmailService.Messages.Commands;

namespace WelcomeEmailService
{
    public class EmailSender : IHandleMessages<SendVerificationEmailCmd>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmailSender));

        public void Handle(SendVerificationEmailCmd message)
        {
            log.InfoFormat("Sending welcome email to {0}", message.EmailAddress);
        }
    }
}
