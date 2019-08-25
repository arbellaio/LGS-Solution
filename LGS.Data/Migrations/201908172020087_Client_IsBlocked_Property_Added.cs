namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Client_IsBlocked_Property_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "IsBlocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "IsBlocked");
        }
    }
}
