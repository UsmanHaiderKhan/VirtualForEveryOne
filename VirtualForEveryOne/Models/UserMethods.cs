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
    }
}
