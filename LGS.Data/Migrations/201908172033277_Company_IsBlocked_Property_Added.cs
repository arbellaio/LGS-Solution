namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Company_IsBlocked_Property_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "IsBlocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "IsBlocked");
        }
    }
}
