namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyChange4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Survey", "Answer4", c => c.Int(nullable: false));
            AlterColumn("dbo.Survey", "Answer1", c => c.String());
            AlterColumn("dbo.Survey", "Answer2", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Survey", "Answer2", c => c.Int(nullable: false));
            AlterColumn("dbo.Survey", "Answer1", c => c.Int(nullable: false));
            DropColumn("dbo.Survey", "Answer4");
        }
    }
}
