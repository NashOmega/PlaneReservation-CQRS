namespace Services.Jobs.Interfaces
{
    public  interface IEmailService
    {
        void SendWelcomeEmail(string email, string name);
        void SedGettingStartedEmail(string email, string name);
    }
}
