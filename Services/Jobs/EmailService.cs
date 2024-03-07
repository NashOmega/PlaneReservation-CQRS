using Services.Jobs.Interfaces;

namespace Services.Jobs
{
    public class EmailService : IEmailService
    {
        public void SedGettingStartedEmail(string email, string name)
        {
            Console.WriteLine($"This will send a welcome email to ${name} using the following email ${email}");


        }

        public void SendWelcomeEmail(string email, string name)
        {
            Console.WriteLine($"This will send a getting started email to ${name} using the following email ${email}");

        }
    }
}
