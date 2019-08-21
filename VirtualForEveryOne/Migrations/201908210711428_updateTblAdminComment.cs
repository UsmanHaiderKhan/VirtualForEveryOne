namespace VirtualForEveryOne.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTblAdminComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "admincomments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reports", "admincomments");
        }
    }
}
