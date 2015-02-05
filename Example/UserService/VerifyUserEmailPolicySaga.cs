using System;
using NServiceBus.Logging;
using NServiceBus.Saga;
using UserService.Messages.Commands;
using WelcomeEmailService.Messages.Commands;

namespace UserService
{
    public class VerifyUserEmailPolicySaga : Saga<VerifyUserEmailPolicySaga.VerifyUserEmailPolicyData>,
        IAmStartedByMessages<CreateNewUserCmd>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (VerifyUserEmailPolicySaga));

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<VerifyUserEmailPolicyData> mapper)
        {
            mapper.ConfigureMapping<CreateNewUserCmd>(msg => msg.EmailAddress).ToSaga(data => data.EmailAddress);
            mapper.ConfigureMapping<UserVerifyingEmailCmd>(msg => msg.EmailAddress).ToSaga(data => data.EmailAddress);
        }

        public void Handle(CreateNewUserCmd message)
        {
            this.Data.Name = message.Name;
            this.Data.EmailAddress = message.EmailAddress;
            this.Data.VerificationCode = Guid.NewGuid().ToString("n").Substring(0, 4);
            Bus.Send(new SendVerificationEmailCmd
            {
                Name = message.Name,
                EmailAddress = message.EmailAddress,
                VerificationCode = Data.VerificationCode,
                IsReminder = false
            });
        }

        public void Handle(UserVerifyingEmailCmd message)
        {
            if (message.VerificationCode == this.Data.VerificationCode)
            {
                Bus.Send(new CreateNewUserWithVerifiedEmailCmd
                {
                    EmailAddress = this.Data.EmailAddress,
                    Name = this.Data.Name
                });
                this.MarkAsComplete();
            }
        }

        public class VerifyUserEmailPolicyData : ContainSagaData
        {
            public virtual string Name { get; set; }

            [Unique]
            public virtual string EmailAddress { get; set; }

            public virtual string VerificationCode { get; set; }
        }
    }
}