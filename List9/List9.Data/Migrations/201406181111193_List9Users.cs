namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class List9Users : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Tasks", new[] { "User_Id" });
            DropColumn("dbo.Tasks", "List9UserId");
            RenameColumn(table: "dbo.Tasks", name: "User_Id", newName: "List9UserId");
            AlterColumn("dbo.Tasks", "List9UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tasks", "List9UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tasks", new[] { "List9UserId" });
            AlterColumn("dbo.Tasks", "List9UserId", c => c.Int());
            RenameColumn(table: "dbo.Tasks", name: "List9UserId", newName: "User_Id");
            AddColumn("dbo.Tasks", "List9UserId", c => c.Int());
            CreateIndex("dbo.Tasks", "User_Id");
        }
    }
}
