namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "UserId", "dbo.Users");
            DropIndex("dbo.Tasks", new[] { "UserId" });
            AddColumn("dbo.Tasks", "List9UserId", c => c.Int());
            AddColumn("dbo.Tasks", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tasks", "User_Id");
            AddForeignKey("dbo.Tasks", "User_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Tasks", "UserId");
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Mobile = c.String(),
                        Email = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        LastEditDate = c.DateTime(),
                        LastEditedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tasks", "UserId", c => c.Int());
            DropForeignKey("dbo.Tasks", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Tasks", new[] { "User_Id" });
            DropColumn("dbo.Tasks", "User_Id");
            DropColumn("dbo.Tasks", "List9UserId");
            CreateIndex("dbo.Tasks", "UserId");
            AddForeignKey("dbo.Tasks", "UserId", "dbo.Users", "Id");
        }
    }
}
