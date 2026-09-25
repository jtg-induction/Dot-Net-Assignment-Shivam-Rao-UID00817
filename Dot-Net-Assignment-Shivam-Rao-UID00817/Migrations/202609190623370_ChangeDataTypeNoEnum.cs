namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ChangeDataTypeNoEnum : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.Orders DROP CONSTRAINT Chk_status");
            Sql("ALTER TABLE dbo.Users DROP CONSTRAINT Chk_role");
            AddColumn("dbo.Users", "roles2", c => c.Int(nullable: false, defaultValue: 1));
            AlterColumn("dbo.Orders", "status", c => c.Int(nullable: false, defaultValue: 1));
        }

        public override void Down()
        {
            AlterColumn("dbo.Orders", "status", c => c.String(nullable: false));
            DropColumn("dbo.Users", "roles2");
            Sql("ALTER TABLE Users ADD CONSTRAINT Chk_role CHECK(role IN ('customer', 'owner', 'admin'))");
            Sql("ALTER TABLE Orders ADD CONSTRAINT Chk_status CHECK(status IN ('Placed', 'Accepted', 'Rejected', 'Dispatched', 'Delivered', 'Cancelled'))");
        }
    }
}
