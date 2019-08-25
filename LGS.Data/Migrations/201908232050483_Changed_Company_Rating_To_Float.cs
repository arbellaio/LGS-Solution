namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changed_Company_Rating_To_Float : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "Ratings", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "Ratings", c => c.Int(nullable: false));
        }
    }
}
