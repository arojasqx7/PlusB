﻿using Quartz;
using Quartz.Impl;

namespace UI.Jobs
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            //First Job (Performance Evaluation
            IJobDetail Job = JobBuilder.Create<performanceEvalutionJob>().Build();
            ITrigger Trigger = TriggerBuilder.Create()
            .WithIdentity("firstTrigger", "group1")
            .WithDailyTimeIntervalSchedule
              (s =>
                 s.WithIntervalInHours(24)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 55))
              )
            .Build();

            //Second Job (KPI Daily evaluation)
            IJobDetail KPIJob = JobBuilder.Create<KPIEvaluationJob>().Build();
            ITrigger KPITrigger = TriggerBuilder.Create()
            .WithIdentity("secondTrigger", "group1")
            .WithDailyTimeIntervalSchedule
              (s =>
                 s.WithIntervalInHours(24)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 56))
              )
            .Build();

            //Third Job (Automated Routing)
            IJobDetail routingJob = JobBuilder.Create<automaticRoutingJob>().Build();
            ITrigger routingTrigger = TriggerBuilder.Create()
            .WithIdentity("thirdTrigger", "group1")
            .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(20)
                .RepeatForever())
            .Build();

            //Job Schedulers Firing
            scheduler.ScheduleJob(Job, Trigger); // Schedule First Job
            scheduler.ScheduleJob(KPIJob,KPITrigger); //Schedule Second Job
            scheduler.ScheduleJob(routingJob, routingTrigger); //Schedule Third Job
        }
    }
}