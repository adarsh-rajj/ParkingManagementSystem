namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VehicleRegistrations", "BlockId", "dbo.ParkingBlocks");
            DropIndex("dbo.VehicleRegistrations", new[] { "BlockId" });
            RenameColumn(table: "dbo.VehicleRegistrations", name: "BlockId", newName: "ParkingBlock_BlockId");
            AlterColumn("dbo.VehicleRegistrations", "ParkingBlock_BlockId", c => c.Int());
            CreateIndex("dbo.VehicleRegistrations", "ParkingBlock_BlockId");
            AddForeignKey("dbo.VehicleRegistrations", "ParkingBlock_BlockId", "dbo.ParkingBlocks", "BlockId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleRegistrations", "ParkingBlock_BlockId", "dbo.ParkingBlocks");
            DropIndex("dbo.VehicleRegistrations", new[] { "ParkingBlock_BlockId" });
            AlterColumn("dbo.VehicleRegistrations", "ParkingBlock_BlockId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.VehicleRegistrations", name: "ParkingBlock_BlockId", newName: "BlockId");
            CreateIndex("dbo.VehicleRegistrations", "BlockId");
            AddForeignKey("dbo.VehicleRegistrations", "BlockId", "dbo.ParkingBlocks", "BlockId", cascadeDelete: true);
        }
    }
}
