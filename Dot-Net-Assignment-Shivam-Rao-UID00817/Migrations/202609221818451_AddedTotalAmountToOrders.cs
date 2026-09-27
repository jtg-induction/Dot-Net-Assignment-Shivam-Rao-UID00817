namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedTotalAmountToOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropColumn("dbo.Orders", "TotalAmount");
        }
    }
}
