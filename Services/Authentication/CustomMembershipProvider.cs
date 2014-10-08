using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using DataModel;

namespace Services.Authentication
{
    //Implements custom membership provider
    public class CustomMembershipProvider : MembershipProvider, IProcessingUser
    {
        private readonly Dictionary<MembershipCreateStatus, string> _membershipError;

        public CustomMembershipProvider()
        {
            _membershipError = new Dictionary<MembershipCreateStatus, string>();
            this.InitializeErrorMessages();
        }
        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;

            using (SocialFundEntities db = new SocialFundEntities())
            {
                try
                {
                    User user = (from u in db.User
                                 where u.Name == username
                                 select u).FirstOrDefault();

                    if ((user != null) & Crypto.VerifyHashedPassword(user.Password, password))
                    {
                        isValid = true;
                    }
                }
                catch
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            using (SocialFundEntities db = new SocialFundEntities())
            {
                try
                {
                    User user = (from u in db.User
                                 where u.Name == username
                                 select u).FirstOrDefault();

                    if (user != null)
                    {
                        var membershipUser = new MembershipUser("CustomMembershipProvider", username, null, null, null, null, false, false,
                                                                 DateTime.Now, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                        return membershipUser;
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public MembershipCreateStatus CreateUser(string userNick, 
                                         string fName, string mName, string lName,
                                         string password, 
                                         string email, 
                                         string phone, 
                                         string address, 
                                         bool isNotif)
        {

            MembershipUser membershipUser = GetUser(userNick, false);

            if (membershipUser == null)
            {
                try
                {
                    using (SocialFundEntities db = new SocialFundEntities())
                    {
                        var user = new User()
                        {
                            Name = userNick,
                            FirstName = fName,
                            MiddleName = mName,
                            LastName = lName,
                            Password = Crypto.HashPassword(password),
                            Email = email,
                            Address = address,
                            Phone = phone,
                            IsNotif = isNotif
                        };

                        db.User.Add(user);
                        db.SaveChanges();

                        //membershipUser = GetUser(userNick, false);
                        return MembershipCreateStatus.Success;
                    }
                }
                catch { return MembershipCreateStatus.ProviderError; }
            }

            return MembershipCreateStatus.DuplicateUserName;
        }

        public override void UpdateUser(MembershipUser user)
        {
            using (SocialFundEntities db = new SocialFundEntities())
            {
                var original = (from u in db.User
                                where u.Name == user.UserName
                                select u).FirstOrDefault();
                if (original != null)
                {
                    original.Email = user.Email;
                    db.SaveChanges();
                }
            }
        }

        public User GetUserInformationByName(string username)
        {
            using (SocialFundEntities db = new SocialFundEntities())
            {
                try
                {
                    User user = (from u in db.User.Include("Group_User")
                                 where u.Name == username
                                 select u).FirstOrDefault();

                    if (user != null)
                    {
                        user.Group = (from gr in user.Group_User select gr.Group).ToList();
                        return user;
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }


        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        private void InitializeErrorMessages()
        {
            _membershipError.Add(
                          MembershipCreateStatus.DuplicateEmail,
                          "Имя пользователя для данного адреса электронной почты уже существует. Введите другой адрес электронной почты.");
            _membershipError.Add(
                           MembershipCreateStatus.DuplicateUserName,
                           "Username already exists. Please enter a different username.");
            _membershipError.Add(
                          MembershipCreateStatus.InvalidEmail,
                          "Недопустимый адрес электронной почты. Проверьте значение поля и повторите попытку.");
            _membershipError.Add(
                          MembershipCreateStatus.InvalidUserName,
                          "Предоставленное имя пользователя недействительно (имя не должно быть больше 10 символов). Проверьте значение поля и повторите попытку.");
            _membershipError.Add(
                          MembershipCreateStatus.InvalidPassword,
                          "Предоставленный пароль недействителен. Введите действительный пароль.");
            _membershipError.Add(
                           MembershipCreateStatus.InvalidAnswer,
                           "Номер телефона не действительный. Введите правельный номер телефона.");
            _membershipError.Add(
                           MembershipCreateStatus.ProviderError,
                           "Unknown error. Check your entry and try again. If the problem persists, contact your system administrator.");
        }


        public string GetErrorMessage(MembershipCreateStatus status)
        {
            string result;
            if (_membershipError.ContainsKey(status))
            {
                result = _membershipError[status];
            }
            else
            {
                result = "Unknown error. Check your entry and try again. If the problem persists, contact your system administrator.";
            }
            return result;
        }
    }
}
