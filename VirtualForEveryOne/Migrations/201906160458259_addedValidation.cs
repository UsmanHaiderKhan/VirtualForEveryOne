namespace VirtualForEveryOne.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Groups", "groupname", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Groups", "groupadmin", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Groups", "info", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "username", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "fullname", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Users", "email", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "skills", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "skills", c => c.String());
            AlterColumn("dbo.Users", "email", c => c.String());
            AlterColumn("dbo.Users", "fullname", c => c.String());
            AlterColumn("dbo.Users", "username", c => c.String());
            AlterColumn("dbo.Groups", "info", c => c.String());
            AlterColumn("dbo.Groups", "groupadmin", c => c.String());
            AlterColumn("dbo.Groups", "groupname", c => c.String());
        }
    }
}
