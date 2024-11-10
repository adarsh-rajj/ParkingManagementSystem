namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blockNo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VehicleRegistrations", "ParkingBlock_BlockId", "dbo.ParkingBlocks");
            DropIndex("dbo.VehicleRegistrations", new[] { "ParkingBlock_BlockId" });
            AddColumn("dbo.VehicleRegistrations", "BlockNo", c => c.String());
            DropColumn("dbo.VehicleRegistrations", "ParkingBlock_BlockId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VehicleRegistrations", "ParkingBlock_BlockId", c => c.Int());
            DropColumn("dbo.VehicleRegistrations", "BlockNo");
            CreateIndex("dbo.VehicleRegistrations", "ParkingBlock_BlockId");
            AddForeignKey("dbo.VehicleRegistrations", "ParkingBlock_BlockId", "dbo.ParkingBlocks", "BlockId");
        }
    }
}
