namespace LGS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_New_Columns_Delivery_Notification_LeadLimitUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "DeliveryInterval", c => c.Int(nullable: false));
            AlterColumn("dbo.Companies", "NotificationMode", c => c.Int(nullable: false));
            AlterColumn("dbo.Companies", "LeadLimit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "LeadLimit", c => c.Int());
            AlterColumn("dbo.Companies", "NotificationMode", c => c.Int());
            AlterColumn("dbo.Companies", "DeliveryInterval", c => c.Int());
        }
    }
}
