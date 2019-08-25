namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_Status_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserStatus");
        }
    }
}
