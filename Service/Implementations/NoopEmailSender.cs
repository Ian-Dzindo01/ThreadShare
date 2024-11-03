using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using System;

namespace ThreadShare.Service.Implementations
{
    public class NoopEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine("Stub email sending function");
            return Task.CompletedTask;
        }
    }
}
