namespace HappyMVCAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LateSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MoneyPerLate = c.Double(nullable: false),
                        PushPerLate = c.Int(nullable: false),
                        SecondRate = c.Int(nullable: false),
                        ThirdRate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LateSettings");
        }
    }
}
