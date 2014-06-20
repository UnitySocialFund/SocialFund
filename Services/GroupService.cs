using Common;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GroupService
    {
        public Group GetGroup(int groupId)
        {
            var group = new Group();

            using (var db = new SocialFundEntities())
            {
                var tmpGroup = db.Group.FirstOrDefault(g => g.Id == groupId);

                tmpGroup.User = db.User.FirstOrDefault(u => u.Id == tmpGroup.OwnerId);

                if (tmpGroup != null)
                {
                    group = tmpGroup;
                }
            }

            return group;
        }

        public List<Group> GetGroupForUser(string userName)
        {
            var result = new List<Group>();
            using (var db = new SocialFundEntities())
            {
                var user = db.User.Where(u => u.Name == userName).FirstOrDefault();
                if (user == null)
                {
                    return result;
                }

                var groups = db.Group.ToList();
                if (groups == null)
                {
                    return result;
                }

                foreach (var group in groups)
                {
                    foreach(var groupUser in group.Group_User)
                    {
                        if (groupUser.UserId == user.Id)
                        {
                            result.Add(group);
                        }
                    }
                }
            }

            return result;
        }

        public List<User> GetUserNotInGroup(int groupId)
        {
            var users = new List<User>();
            using (var db = new SocialFundEntities())
            {
                var allUsers = db.User.ToList();
                var groupUsers = db.Group_User.Where(g => g.GroupId == groupId).ToList();

                foreach (var groupUser in groupUsers)
                {
                    var user = db.User.FirstOrDefault(u => u.Id == groupUser.UserId);
                    allUsers.Remove(user);
                }
                users = allUsers;
            }

            return users;
        }

        public List<User> GetUsersForGroup(int groupId)
        {
            var users = new List<User>();
            using (var db = new SocialFundEntities())
            {
                var groupUsers = db.Group_User.Where(g => g.GroupId == groupId).ToList();
                foreach (var groupUser in groupUsers)
                {
                    users.Add(groupUser.User);
                }
            }

            return users;
        }

        public void CreateGroup(Group group,string userName)
        {
            using (var db = new SocialFundEntities())
            {
                var user = db.User.FirstOrDefault(u => u.Name == userName);
                if (user == null)
                {
                    return;
                }

                group.User = user;
                db.Group.Add(group);

                var groupUser = new Group_User();
                groupUser.User = user;
                groupUser.Group = group;
                db.Group_User.Add(groupUser);

                db.SaveChanges();
            }
        }

        public void CreateGroupUser(int groupId, int userId)
        {
            using (var db = new SocialFundEntities())
            {
                var groupUser = new Group_User();
                
                groupUser.Group = db.Group.FirstOrDefault(g => g.Id == groupId);
                groupUser.User = db.User.FirstOrDefault(u => u.Id == userId);
                groupUser.GroupId = groupId;
                groupUser.UserId = userId;

                db.Group_User.Add(groupUser);
                db.SaveChanges();
            }
        }

        public void DeleteUserFromGroup(int groupId, int userId)
        {
            using (var db = new SocialFundEntities())
            {
                var reccord = db.Group_User.Where(x => x.GroupId == groupId && x.UserId == userId).SingleOrDefault();
                db.Group_User.Remove(reccord);
                db.SaveChanges();
            }
        }

        public void EditGroupDetails(Group gr)
        {
            using (var db = new SocialFundEntities())
            {
                var group = db.Group.Single(x => x.Id == gr.Id);
                group.Name = gr.Name;
                group.OwnerId = gr.OwnerId;
                db.SaveChanges();
            }
        }

        public void Spam(int groupId, string title, string message)
        {
            var users = GetUsersForGroup(groupId).Where(x => x.IsNotif == true).ToList(); 
            foreach (var user in users)
            {
                if (MailSender.ValidateEmail(user.Email))
                {
                    MailSender sender = new MailSender();
                    sender.InitializeMailMessage("unitysocialfund@gmail.com", user.Email, "Feedback from Social Fund: " + title, message);
                    sender.SendSmtp(false);
                }
            }
        }
    }
}
