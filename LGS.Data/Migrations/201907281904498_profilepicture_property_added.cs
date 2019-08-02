namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profilepicture_property_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "ProfilePhoto", c => c.String());
            AddColumn("dbo.SubAdmins", "ProfilePhoto", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubAdmins", "ProfilePhoto");
            DropColumn("dbo.Clients", "ProfilePhoto");
        }
    }
}
