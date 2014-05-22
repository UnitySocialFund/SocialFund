using System;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class EmailSenderTest
    {
        [TestMethod]
        public void SendEmailWithAuthentication()
        {
            Assert.Inconclusive();

            MailSender sender = new MailSender();
            sender.SetCredential("snake@playasoftware.com", ",jdleq");
            sender.InitializeMailMessage("test@mail.ru", "snake1982309@hotmail.com", "With Authentication", "Test");
            sender.SendSmtp("smtp.gmail.com", 587, false);
        }

        [TestMethod] 
        public void SendEmailWithoutAuthentication()
        {
           // Assert.Inconclusive();

            MailSender sender = new MailSender();
            sender.InitializeMailMessage("test@mail.ru", "lreed@ipswitch.com", "Without Authentication", "Test");
            sender.SendSmtp("smtptest.ipswitch.com", 25, false);
        }

        [TestMethod]
        public void SendEmailSettingsFromConfiguration()
        {
            //Assert.Inconclusive();

            MailSender sender = new MailSender();
            sender.SetCredential("unitysocialfund@mail.ru", "Install_new23");
            sender.InitializeMailMessage("test@mail.ru", "snake1982309@hotmail.com", "From Configuration", "Test");
            sender.SendSmtp(false);
        }
    }
}
