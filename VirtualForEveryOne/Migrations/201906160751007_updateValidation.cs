namespace VirtualForEveryOne.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "fullname", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "fullname", c => c.String(nullable: false, maxLength: 25));
        }
    }
}
