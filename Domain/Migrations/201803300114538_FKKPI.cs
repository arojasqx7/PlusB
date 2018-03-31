namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKKPI : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.KPI", "Consultant_ID", "dbo.Consultant");
            DropIndex("dbo.KPI", new[] { "Consultant_ID" });
            DropColumn("dbo.KPI", "IdConsultant");
            RenameColumn(table: "dbo.KPI", name: "Consultant_ID", newName: "IdConsultant");
            AlterColumn("dbo.KPI", "IdConsultant", c => c.Int(nullable: false));
            CreateIndex("dbo.KPI", "IdConsultant");
            AddForeignKey("dbo.KPI", "IdConsultant", "dbo.Consultant", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KPI", "IdConsultant", "dbo.Consultant");
            DropIndex("dbo.KPI", new[] { "IdConsultant" });
            AlterColumn("dbo.KPI", "IdConsultant", c => c.Int());
            RenameColumn(table: "dbo.KPI", name: "IdConsultant", newName: "Consultant_ID");
            AddColumn("dbo.KPI", "IdConsultant", c => c.Int(nullable: false));
            CreateIndex("dbo.KPI", "Consultant_ID");
            AddForeignKey("dbo.KPI", "Consultant_ID", "dbo.Consultant", "ID");
        }
    }
}
