using Quartz;
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
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 42))
              )
            .Build();

            scheduler.ScheduleJob(Job, Trigger); // Schedule First Job
            scheduler.ScheduleJob(KPIJob,KPITrigger); //Schedule Second Job
        }
    }
}