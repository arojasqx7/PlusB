namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPerfEvaluationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PerformanceEvaluation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        idConsultant = c.Int(nullable: false),
                        TotalResolvedIncidents = c.Int(nullable: false),
                        TotalAssignedIncidents = c.Int(nullable: false),
                        TechWeightAverage = c.Double(nullable: false),
                        TotalHoursTaken = c.Double(nullable: false),
                        PerformanceAverage = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Consultant", t => t.idConsultant, cascadeDelete: true)
                .Index(t => t.idConsultant);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PerformanceEvaluation", "idConsultant", "dbo.Consultant");
            DropIndex("dbo.PerformanceEvaluation", new[] { "idConsultant" });
            DropTable("dbo.PerformanceEvaluation");
        }
    }
}
