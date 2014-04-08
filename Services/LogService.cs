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
                    log.Group_User.Group = GetGroup(log.Group_User.GroupId);
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
                    log.Group_User.Group = GetGroup(log.Group_User.GroupId);
                }

                if (tmpHistory != null)
                {
                    logsHistory = tmpHistory;
                }
            }
            return logsHistory;
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

        public void AddLog(Log log, int groupId, int userId)
        {
            var groupUser = new Group_User();

            using (var db = new SocialFundEntities())
            {
                var tmpGroupUser = db.Group_User.FirstOrDefault(gu => gu.GroupId == groupId && gu.UserId == userId);
                if (tmpGroupUser != null)
                {
                    groupUser = tmpGroupUser;
                }

                log.Group_User = groupUser;
                db.Log.Add(log);
                db.SaveChanges();
            }
        }

        private Group GetGroup(int groupId)
        {
            var group = new Group();

            using (var db = new SocialFundEntities())
            {
                var tmpGroup = db.Group.FirstOrDefault(g => g.Id == groupId);
                if (tmpGroup != null)
                {
                    group = tmpGroup;
                }
            }

            return group;
        }
    }
}
