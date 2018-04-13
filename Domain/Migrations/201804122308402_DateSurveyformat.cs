namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateSurveyformat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Survey", "DateSent", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Survey", "DateSent", c => c.DateTime(nullable: false));
        }
    }
}
