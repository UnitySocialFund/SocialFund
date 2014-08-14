using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LogService
    {
        public LogService()
        {
            _groupService = new GroupService();
        }

        private GroupService _groupService;

        public decimal GetCurrentBalance(int groupId)
        {
            decimal balance = 0;
            
            using (var db = new SocialFundEntities())
            {
                var logsForGroup = db.Log.Where(l => l.Group_User.GroupId == groupId);
                foreach (var log in logsForGroup)
                {
                    balance += log.Coins;
                }
            }
            return balance;
        }

        public List<Log> GetGroupHistory(int groupId)
        {
            var logsHistory = new List<Log>();
            using (var db = new SocialFundEntities())
            {
                var tmpHistory = db.Log.Where(l => l.Group_User.GroupId == groupId).ToList();
                
                foreach (var log in tmpHistory)
                {
                    log.Group_User.User = GetUser(log.Group_User.UserId);
                    log.Group_User.Group = _groupService.GetGroup(log.Group_User.GroupId);
                }

                if (tmpHistory != null)
                {
                    logsHistory = tmpHistory;
                }
            }
            return logsHistory;
        }

        public List<Log>  GetUserHistory(int userId)
        {
            var logsHistory = new List<Log>();
            using (var db = new SocialFundEntities())
            {
                var tmpHistory = db.Log.Where(l => l.Group_User.UserId == userId).ToList();
                
                foreach (var log in tmpHistory)
                {
                    log.Group_User.User = GetUser(log.Group_User.UserId);
                    log.Group_User.Group = _groupService.GetGroup(log.Group_User.GroupId);
                }

                if (tmpHistory != null)
                {
                    logsHistory = tmpHistory;
                }
            }
            return logsHistory;
        }

        public void AddLog(Log log, int groupId, string userName)
        {
            using (var db = new SocialFundEntities())
            {
                var user = db.User.FirstOrDefault(u => u.Name == userName);
                if (user == null)
                {
                    return;
                }

                var groupUser = db.Group_User.Where(gu => gu.GroupId == groupId && gu.UserId == user.Id).FirstOrDefault();
                if (groupUser == null)
                {
                    return;
                }

                log.Date = DateTime.Now;
                log.Group_User = groupUser;
                db.Log.Add(log);
                db.SaveChanges();
            }
        }

        public int GetUserId(string userName)
        {
            var user = new User();

            using (var db = new SocialFundEntities())
            {
                var tmpUser = db.User.FirstOrDefault(u => u.Name == userName);
                if (tmpUser != null)
                {
                    user = tmpUser;
                }
            }

            return user.Id;
        }

        public bool IsUserMember(int groupId, int userId)
        {
            using (var db = new SocialFundEntities())
            {
                var tmp = db.Group_User.FirstOrDefault(u => u.GroupId == groupId && u.UserId == userId);
                if (tmp == null)
                {
                    return false;
                }
                return true;
            }
        }

        private User GetUser(int userId)
        {
            var user = new User();

            using (var db = new SocialFundEntities())
            {
                var tmpUser = db.User.FirstOrDefault(u => u.Id == userId);
                if (tmpUser != null)
                {
                    user = tmpUser;
                }
            }

            return user;
        }
    }
}
