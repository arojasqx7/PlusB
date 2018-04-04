namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KPIEvaluationEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KPIEvaluation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        idConsultant = c.Int(nullable: false),
                        idKPI = c.Int(nullable: false),
                        Score = c.Single(nullable: false),
                        Range = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Consultant", t => t.idConsultant, cascadeDelete: true)
                .ForeignKey("dbo.KPI", t => t.idKPI, cascadeDelete: true)
                .Index(t => t.idConsultant)
                .Index(t => t.idKPI);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KPIEvaluation", "idKPI", "dbo.KPI");
            DropForeignKey("dbo.KPIEvaluation", "idConsultant", "dbo.Consultant");
            DropIndex("dbo.KPIEvaluation", new[] { "idKPI" });
            DropIndex("dbo.KPIEvaluation", new[] { "idConsultant" });
            DropTable("dbo.KPIEvaluation");
        }
    }
}
