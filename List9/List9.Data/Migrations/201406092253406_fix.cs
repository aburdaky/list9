namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "TaskCategories_Id", "dbo.TaskCategories");
            DropIndex("dbo.Tasks", new[] { "TaskCategories_Id" });
            RenameColumn(table: "dbo.Tasks", name: "TaskCategories_Id", newName: "TaskCategoryId");
            AlterColumn("dbo.Tasks", "TaskCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tasks", "TaskCategoryId");
            AddForeignKey("dbo.Tasks", "TaskCategoryId", "dbo.TaskCategories", "Id", cascadeDelete: true);
            DropColumn("dbo.Tasks", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "CategoryId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Tasks", "TaskCategoryId", "dbo.TaskCategories");
            DropIndex("dbo.Tasks", new[] { "TaskCategoryId" });
            AlterColumn("dbo.Tasks", "TaskCategoryId", c => c.Int());
            RenameColumn(table: "dbo.Tasks", name: "TaskCategoryId", newName: "TaskCategories_Id");
            CreateIndex("dbo.Tasks", "TaskCategories_Id");
            AddForeignKey("dbo.Tasks", "TaskCategories_Id", "dbo.TaskCategories", "Id");
        }
    }
}
