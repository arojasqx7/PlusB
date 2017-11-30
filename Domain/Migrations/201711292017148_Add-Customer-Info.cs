namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        CompanyId = c.String(),
                        CompanyDescription = c.String(),
                        ManagerName = c.String(),
                        Country = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        EmailAddress = c.String(),
                        SupportType = c.String(),
                        InitialDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Customer");
        }
    }
}
