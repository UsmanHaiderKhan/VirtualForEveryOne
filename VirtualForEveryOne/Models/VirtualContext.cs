using System.Data.Entity;

namespace VirtualForEveryOne.Models
{
    public class VirtualContext : DbContext
    {
        public VirtualContext() : base("name=constr")
        {

        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Friends> Friendses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<SharedPost> SharedPosts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLike> UserLikes { get; set; }


    }
}
