namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIDSLAfromConsultant : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Consultant", "SLA_ID", "dbo.SLA");
            DropIndex("dbo.Consultant", new[] { "SLA_ID" });
            DropColumn("dbo.Consultant", "IdSLA");
            DropColumn("dbo.Consultant", "SLA_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Consultant", "SLA_ID", c => c.Int());
            AddColumn("dbo.Consultant", "IdSLA", c => c.Int());
            CreateIndex("dbo.Consultant", "SLA_ID");
            AddForeignKey("dbo.Consultant", "SLA_ID", "dbo.SLA", "ID");
        }
    }
}
