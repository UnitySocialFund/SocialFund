using System.Data.Entity.Migrations;
using System.Linq;
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

        public void UpdateUser(User user)
        {
            using (var db = new SocialFundEntities())
            {
                var oldUser = db.User.Single(x => x.Id == user.Id);
                oldUser.Address = user.Address;
                oldUser.Email = user.Email;
                oldUser.Name = user.Name;
                oldUser.Phone = user.Phone;
                oldUser.IsNotif = user.IsNotif;
                db.User.AddOrUpdate(oldUser);
                db.SaveChanges();
            }
        }
    }
}
