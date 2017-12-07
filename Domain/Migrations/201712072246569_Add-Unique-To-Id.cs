namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueToId : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Consultant", "IdNumber", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Consultant", new[] { "IdNumber" });
        }
    }
}
