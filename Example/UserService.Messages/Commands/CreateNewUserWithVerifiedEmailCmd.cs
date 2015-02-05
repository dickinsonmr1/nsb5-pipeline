using NServiceBus;

namespace UserService.Messages.Commands
{
    public class CreateNewUserWithVerifiedEmailCmd
    {
        public string EmailAddress { get; set; }
        public string Name { get; set; }
    }
}
