﻿using System;
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

        public MembershipUser CreateUser(string userNick, 
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

                        membershipUser = GetUser(userNick, false);
                        return membershipUser;
                    }
                }
                catch { return null; }
            }
            return null;
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


    }
}
