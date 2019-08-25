namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_Status_Removed : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "UserStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserStatus", c => c.Int(nullable: false));
        }
    }
}
