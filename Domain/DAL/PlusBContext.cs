﻿using Domain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Domain.DAL
{
    public class PlusBContext : DbContext
    {
        public PlusBContext() : base("PlusBContext")
        {
        }
        public DbSet<Technology> Technologies {get; set;}
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<Severity> Severities { get; set; }
        public DbSet<Impact> Impacts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<TicketActivity> TicketActivities { get; set; }
        public DbSet<SLA> SLAS { get; set; }
        public DbSet<KnowledgeBase> KnowledgeBases { get; set; }
        public DbSet<KPI> KPIS { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<CustomerSLA> CustomerSLAS { get; set; }
        public DbSet<PerformanceEvaluation> PerformanceEvalutions { get; set; }
        public DbSet<ConsultantKPI> ConsultantsKPIs { get; set; }
        public DbSet<KPIEvaluation> KPIEvaluations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            Database.SetInitializer<PlusBContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
