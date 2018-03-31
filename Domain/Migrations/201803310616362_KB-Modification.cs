namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KBModification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KnowledgeBase", "idConsultant", c => c.Int(nullable: false));
            AlterColumn("dbo.KnowledgeBase", "Solution", c => c.String(maxLength: 450));
            CreateIndex("dbo.KnowledgeBase", "Solution", unique: true);
            CreateIndex("dbo.KnowledgeBase", "idConsultant");
            AddForeignKey("dbo.KnowledgeBase", "idConsultant", "dbo.Consultant", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KnowledgeBase", "idConsultant", "dbo.Consultant");
            DropIndex("dbo.KnowledgeBase", new[] { "idConsultant" });
            DropIndex("dbo.KnowledgeBase", new[] { "Solution" });
            AlterColumn("dbo.KnowledgeBase", "Solution", c => c.String());
            DropColumn("dbo.KnowledgeBase", "idConsultant");
        }
    }
}
