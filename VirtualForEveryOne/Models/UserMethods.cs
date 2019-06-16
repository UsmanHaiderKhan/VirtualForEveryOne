using System.Collections.Generic;
using System.Linq;

namespace VirtualForEveryOne.Models
{
    public class UserMethods
    {

        private VirtualContext db = new VirtualContext();

        public User GetUser()
        {
            using (db)
            {
                return (from c in db.Users select c).FirstOrDefault();
            }
        }

        public List<Group> GetAllGroups()
        {
            using (db)
            {
                return (from c in db.Groups select c).ToList();
            }
        }

        public Group GetGroupById(int id)
        {
            using (db)
            {
                return (from c in db.Groups where c.Id == id select c).FirstOrDefault();
            }
        }
    }
}
