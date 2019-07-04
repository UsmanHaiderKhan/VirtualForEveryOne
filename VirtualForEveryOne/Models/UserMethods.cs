using System.Collections.Generic;
using System.Data.Entity;
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

        public List<GroupMember> GetGroupMembers(int groupid)
        {
            using (db)
            {
                return (from v in db.GroupMembers where v.groudid == groupid select v).ToList();
            }
        }

        public Answer GetAnswersById(int id)
        {
            using (db)
            {
                return (from v in db.Answers where v.Id == id select v).SingleOrDefault();
            }
        }

        public void UpdateAnswer(Answer answer)
        {
            using (db)
            {
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
