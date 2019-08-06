namespace VirtualForEveryOne.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applypasswordValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "password", c => c.String(nullable: false, maxLength: 12));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "password", c => c.String(nullable: false));
        }
    }
}
