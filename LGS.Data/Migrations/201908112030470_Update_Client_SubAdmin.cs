namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Client_SubAdmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "FacebookId", c => c.String());
            AddColumn("dbo.Clients", "GoogleId", c => c.String());
            AddColumn("dbo.SubAdmins", "IsBlocked", c => c.Boolean(nullable: false));
            DropColumn("dbo.Clients", "PhoneNumber");
            DropColumn("dbo.SubAdmins", "PhoneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubAdmins", "PhoneNumber", c => c.String());
            AddColumn("dbo.Clients", "PhoneNumber", c => c.String());
            DropColumn("dbo.SubAdmins", "IsBlocked");
            DropColumn("dbo.Clients", "GoogleId");
            DropColumn("dbo.Clients", "FacebookId");
        }
    }
}
