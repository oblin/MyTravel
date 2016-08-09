using System;
using System.Diagnostics;

namespace MyTravel.Services
{
    public class DebugMailService : IMailService
    {
        public void SendMail(string to, string from, string subject, string body)
        {
            Console.WriteLine($"Sending Mail: To: {to} From {from} Subject: {subject}");
        }
    }
}
