namespace VirtualForEveryOne.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        a_username = c.String(),
                        question = c.String(),
                        image = c.String(),
                        ans = c.String(),
                        s_profilepic = c.String(),
                        a_profilepic = c.String(),
                        like = c.Int(),
                        dislike = c.Int(),
                        time = c.DateTime(nullable: false),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Faqs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        faq1 = c.String(),
                        faqAnswer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        userfrirends = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        groudid = c.Int(nullable: false),
                        members = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        groupid = c.Int(nullable: false),
                        groupname = c.String(),
                        groupadmin = c.String(),
                        image = c.String(),
                        info = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        postid = c.String(),
                        username = c.String(),
                        notifier = c.String(),
                        type = c.String(),
                        state = c.Boolean(nullable: false),
                        time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        question = c.String(),
                        image = c.String(),
                        tag = c.String(),
                        anonymous = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        postid = c.String(),
                        comments = c.String(),
                        status = c.String(),
                        time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SharedPosts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        answerid = c.Int(nullable: false),
                        time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.UserLikes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        question = c.String(),
                        image = c.String(),
                        answer = c.String(),
                        like = c.Int(nullable: false),
                        dislike = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        fullname = c.String(),
                        email = c.String(),
                        password = c.String(),
                        dob = c.String(),
                        skills = c.String(),
                        gender = c.String(),
                        bio = c.String(),
                        profilepic = c.String(),
                        coverpic = c.String(),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.UserLikes");
            DropTable("dbo.SharedPosts");
            DropTable("dbo.Reports");
            DropTable("dbo.Posts");
            DropTable("dbo.Notifications");
            DropTable("dbo.Groups");
            DropTable("dbo.GroupMembers");
            DropTable("dbo.Friends");
            DropTable("dbo.Faqs");
            DropTable("dbo.Answers");
            DropTable("dbo.Admins");
        }
    }
}
