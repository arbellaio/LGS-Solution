namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsBlocked_And_IsDeleted_Properties_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Companies", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "IsDeleted");
            DropColumn("dbo.Clients", "IsDeleted");
        }
    }
}
