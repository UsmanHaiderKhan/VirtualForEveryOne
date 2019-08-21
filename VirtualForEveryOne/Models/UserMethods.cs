using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace VirtualForEveryOne.Models
{
    public class UserMethods
    {

        private VirtualContext db = new VirtualContext();
        public void UpdateUser(User user)
        {
            using (db)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public User GetUserByEmail(string email)
        {
            using (db)
            {
                return (from u in db.Users
                        where u.email == email
                        select u).FirstOrDefault();
            }
        }
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
        public Post GetQuestionById(int id)
        {
            using (db)
            {
                return (from v in db.Posts where v.Id == id select v).SingleOrDefault();
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
        public void UpdateQuestion(Post post)
        {
            using (db)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public Report GetReportByUser(string name)
        {

            using (db)
            {
                return (from c in db.Reports where name == c.username select c).FirstOrDefault();
            }
        }
    }
}
