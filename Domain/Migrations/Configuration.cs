namespace Domain.Migrations
{
    using Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Domain.DAL.PlusBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.PlusBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Customers.AddOrUpdate(
              p => p.Id,
              new Customer { Id = 1, CompanyName="PlusB Consulting", CompanyId="ADM", CompanyDescription="Consulting Provider",
                  ManagerName ="Francisco Alvarado", Country="Costa Rica", Address="Escazu", PhoneNumber="22244852",
                  EmailAddress="info@plusb.com", SupportType=null, InitialDate =Convert.ToDateTime("01/01/2000")}
            );

            context.Customers.AddOrUpdate(
              p => p.Id,
              new Customer
              {
                  Id = 2,
                  CompanyName = "Sapiens Software",
                  CompanyId = "CUST1",
                  CompanyDescription = "Software Development",
                  ManagerName = "Alcidez Lopez",
                  Country = "Costa Rica",
                  Address = "San Jose",
                  PhoneNumber = "22244852",
                  EmailAddress = "info@sapiensdev.com",
                  SupportType = null,
                  InitialDate = Convert.ToDateTime("01/01/2000")
              }
            );

            context.Consultants.AddOrUpdate(
                p => p.ID,
                new Consultant
                {
                    ID = 1,
                    FirstName = "Unassigned",
                    LastName = "Unassigned",
                    DateOfBirth = Convert.ToDateTime("01/01/2000"),
                    IdNumber = "1",
                    Gender = "Male",
                    Email = "info@plusb.com",
                    Pais = "Costa Rica",
                    Address = "Escazu",
                    PhoneNumber = "22244852",
                    JobTitle = "Admin",
                    HireDate = Convert.ToDateTime("01/01/2000")
                }
                );
        }
    }
}
