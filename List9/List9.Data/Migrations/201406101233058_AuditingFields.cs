namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuditingFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "CreatedBy", c => c.String());
            AddColumn("dbo.Projects", "LastEditDate", c => c.DateTime());
            AddColumn("dbo.Projects", "LastEditedBy", c => c.String());
            AddColumn("dbo.TaskCategories", "CreatedBy", c => c.String());
            AddColumn("dbo.TaskCategories", "LastEditDate", c => c.DateTime());
            AddColumn("dbo.TaskCategories", "LastEditedBy", c => c.String());
            AddColumn("dbo.Tasks", "CreatedBy", c => c.String());
            AddColumn("dbo.Tasks", "LastEditDate", c => c.DateTime());
            AddColumn("dbo.Tasks", "LastEditedBy", c => c.String());
            AddColumn("dbo.Users", "CreatedBy", c => c.String());
            AddColumn("dbo.Users", "LastEditDate", c => c.DateTime());
            AddColumn("dbo.Users", "LastEditedBy", c => c.String());
            DropColumn("dbo.Projects", "EditedDate");
            DropColumn("dbo.TaskCategories", "EditedDate");
            DropColumn("dbo.Tasks", "EditedDate");
            DropColumn("dbo.Users", "EditedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "EditedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tasks", "EditedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.TaskCategories", "EditedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "EditedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "LastEditedBy");
            DropColumn("dbo.Users", "LastEditDate");
            DropColumn("dbo.Users", "CreatedBy");
            DropColumn("dbo.Tasks", "LastEditedBy");
            DropColumn("dbo.Tasks", "LastEditDate");
            DropColumn("dbo.Tasks", "CreatedBy");
            DropColumn("dbo.TaskCategories", "LastEditedBy");
            DropColumn("dbo.TaskCategories", "LastEditDate");
            DropColumn("dbo.TaskCategories", "CreatedBy");
            DropColumn("dbo.Projects", "LastEditedBy");
            DropColumn("dbo.Projects", "LastEditDate");
            DropColumn("dbo.Projects", "CreatedBy");
        }
    }
}
