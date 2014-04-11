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
                if (tmpGroup != null)
                {
                    group = tmpGroup;
                }
            }

            return group;
        }
    }
}
