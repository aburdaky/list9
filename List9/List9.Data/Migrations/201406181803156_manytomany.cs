namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manytomany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "List9UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Tasks", new[] { "List9UserId" });
            CreateTable(
                "dbo.List9UserProject",
                c => new
                    {
                        List9User_Id = c.String(nullable: false, maxLength: 128),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.List9User_Id, t.Project_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.List9User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.List9User_Id)
                .Index(t => t.Project_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.List9UserProject", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.List9UserProject", "List9User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.List9UserProject", new[] { "Project_Id" });
            DropIndex("dbo.List9UserProject", new[] { "List9User_Id" });
            DropTable("dbo.List9UserProject");
            CreateIndex("dbo.Tasks", "List9UserId");
            AddForeignKey("dbo.Tasks", "List9UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
