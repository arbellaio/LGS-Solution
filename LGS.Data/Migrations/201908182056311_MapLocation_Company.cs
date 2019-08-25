namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MapLocation_Company : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "MapLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "MapLocation");
        }
    }
}
