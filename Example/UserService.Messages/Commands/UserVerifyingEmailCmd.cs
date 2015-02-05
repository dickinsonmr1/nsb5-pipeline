using NServiceBus;

namespace UserService.Messages.Commands
{
    public class UserVerifyingEmailCmd
    {
        public string VerificationCode { get; set; }
        public string EmailAddress { get; set; }
    }
}
