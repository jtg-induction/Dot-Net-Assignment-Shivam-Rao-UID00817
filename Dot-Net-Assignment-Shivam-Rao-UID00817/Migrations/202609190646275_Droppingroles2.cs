namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Droppingroles2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "role");
        }

        public override void Down()
        {
            AddColumn("dbo.Users", "role", c => c.String(nullable: false));
        }
    }
}
