namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_PhoneNumber_FacebookAccessKey_In_Client_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "FacebookUserAccessToken", c => c.String());
            AddColumn("dbo.Clients", "PhoneNumber", c => c.String());
            AddColumn("dbo.Companies", "FacebookPageAccessToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "FacebookPageAccessToken");
            DropColumn("dbo.Clients", "PhoneNumber");
            DropColumn("dbo.Clients", "FacebookUserAccessToken");
        }
    }
}
