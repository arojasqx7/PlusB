namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConsultantKPI : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.KPI", "IdConsultant", "dbo.Consultant");
            DropIndex("dbo.KPI", new[] { "IdConsultant" });
            CreateTable(
                "dbo.ConsultantKPI",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        idConsultant = c.Int(nullable: false),
                        idKPI = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Consultant", t => t.idConsultant, cascadeDelete: true)
                .ForeignKey("dbo.KPI", t => t.idKPI, cascadeDelete: true)
                .Index(t => t.idConsultant)
                .Index(t => t.idKPI);
            
            DropColumn("dbo.KPI", "IdConsultant");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KPI", "IdConsultant", c => c.Int(nullable: false));
            DropForeignKey("dbo.ConsultantKPI", "idKPI", "dbo.KPI");
            DropForeignKey("dbo.ConsultantKPI", "idConsultant", "dbo.Consultant");
            DropIndex("dbo.ConsultantKPI", new[] { "idKPI" });
            DropIndex("dbo.ConsultantKPI", new[] { "idConsultant" });
            DropTable("dbo.ConsultantKPI");
            CreateIndex("dbo.KPI", "IdConsultant");
            AddForeignKey("dbo.KPI", "IdConsultant", "dbo.Consultant", "ID", cascadeDelete: true);
        }
    }
}
