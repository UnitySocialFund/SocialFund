using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataModel;

namespace Services
{
    public class UserService
    {
        public void SandMail(string mail, string title, string message)
        {
            if (MailSender.ValidateEmail(mail))
            {
                MailSender sender = new MailSender();
                sender.InitializeMailMessage("unitysocialfund@gmail.com", mail, "Feedback from Social Fund: " + title, message);
                sender.SendSmtp(false);
            }
        }
    }
}
