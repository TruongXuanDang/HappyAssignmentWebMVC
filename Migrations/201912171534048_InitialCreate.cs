namespace HappyMVCAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classrooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        ClassroomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classrooms", t => t.ClassroomId, cascadeDelete: true)
                .Index(t => t.ClassroomId);
            
            CreateTable(
                "dbo.LateEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LateDate = c.DateTime(nullable: false),
                        LateType = c.Int(nullable: false),
                        LateMoney = c.Double(nullable: false),
                        PushCount = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LateEvents", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Students", "ClassroomId", "dbo.Classrooms");
            DropIndex("dbo.LateEvents", new[] { "StudentId" });
            DropIndex("dbo.Students", new[] { "ClassroomId" });
            DropTable("dbo.LateEvents");
            DropTable("dbo.Students");
            DropTable("dbo.Classrooms");
        }
    }
}
