namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConsultantKPIRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KPI", "IdConsultant", c => c.Int(nullable: false));
            AddColumn("dbo.KPI", "Consultant_ID", c => c.Int());
            CreateIndex("dbo.KPI", "Consultant_ID");
            AddForeignKey("dbo.KPI", "Consultant_ID", "dbo.Consultant", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KPI", "Consultant_ID", "dbo.Consultant");
            DropIndex("dbo.KPI", new[] { "Consultant_ID" });
            DropColumn("dbo.KPI", "Consultant_ID");
            DropColumn("dbo.KPI", "IdConsultant");
        }
    }
}
