namespace WelcomeEmailService.Messages.Commands
{
    public class SendVerificationEmailCmd
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string VerificationCode { get; set; }
        public bool IsReminder { get; set; }
    }
}
