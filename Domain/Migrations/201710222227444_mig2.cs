namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Consultant",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        IdNumber = c.String(),
                        Gender = c.String(),
                        Email = c.String(),
                        Pais = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        JobTitle = c.String(),
                        HireDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Consultant");
        }
    }
}
