namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttachmentEntity : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Impact", new[] { "ImpactName" });
            DropIndex("dbo.Severity", new[] { "SeverityName" });
            DropIndex("dbo.TaskType", new[] { "TaskName" });
            CreateTable(
                "dbo.Attachment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FileContent = c.Binary(),
                        IdTicket = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ticket", t => t.IdTicket, cascadeDelete: true)
                .Index(t => t.IdTicket);
            
            AlterColumn("dbo.Impact", "ImpactName", c => c.String(maxLength: 450));
            AlterColumn("dbo.Severity", "SeverityName", c => c.String(maxLength: 450));
            AlterColumn("dbo.TaskType", "TaskName", c => c.String(maxLength: 450));
            CreateIndex("dbo.Impact", "ImpactName", unique: true);
            CreateIndex("dbo.Severity", "SeverityName", unique: true);
            CreateIndex("dbo.TaskType", "TaskName", unique: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachment", "IdTicket", "dbo.Ticket");
            DropIndex("dbo.TaskType", new[] { "TaskName" });
            DropIndex("dbo.Severity", new[] { "SeverityName" });
            DropIndex("dbo.Impact", new[] { "ImpactName" });
            DropIndex("dbo.Attachment", new[] { "IdTicket" });
            AlterColumn("dbo.TaskType", "TaskName", c => c.String());
            AlterColumn("dbo.Severity", "SeverityName", c => c.String());
            AlterColumn("dbo.Impact", "ImpactName", c => c.String());
            DropTable("dbo.Attachment");
            CreateIndex("dbo.TaskType", "TaskName", unique: true);
            CreateIndex("dbo.Severity", "SeverityName", unique: true);
            CreateIndex("dbo.Impact", "ImpactName", unique: true);
        }
    }
}
