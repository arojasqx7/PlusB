namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueIds3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Technology", "Name", c => c.String(maxLength: 450));
            AlterColumn("dbo.Technology", "Description", c => c.String());
            CreateIndex("dbo.Impact", "ImpactName", unique: true);
            CreateIndex("dbo.Severity", "SeverityName", unique: true);
            CreateIndex("dbo.TaskType", "TaskName", unique: true);
            CreateIndex("dbo.Technology", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Technology", new[] { "Name" });
            DropIndex("dbo.TaskType", new[] { "TaskName" });
            DropIndex("dbo.Severity", new[] { "SeverityName" });
            DropIndex("dbo.Impact", new[] { "ImpactName" });
            AlterColumn("dbo.Technology", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Technology", "Name", c => c.String(nullable: false));
        }
    }
}
