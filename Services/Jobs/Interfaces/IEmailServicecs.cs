namespace Services.Jobs.Interfaces
{
    public  interface IEmailServicecs
    {
        void SendWelcomeEmail(string email, string name);
        void SedGettingStartedEmail(string email, string name);
    }
}
