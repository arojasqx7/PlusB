using Quartz;
using Quartz.Impl;
using UI.Background_Jobs;

namespace UI.Jobs
{
    public class JobScheduler
    {
        private static readonly JobSchedulerOptions DefaultOptions = new JobSchedulerOptions();

        public static void Start()
        {
            Start(DefaultOptions);
        }

        public static void Start(JobSchedulerOptions options)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail Job = JobBuilder.Create<performanceEvalutionJob>().Build();
            ITrigger Trigger = TriggerBuilder.Create()
            .WithIdentity("firstTrigger", "group1")
            .WithDailyTimeIntervalSchedule
              (s =>
                 s.WithIntervalInHours(24)
                .OnEveryDay()
                .StartingDailyAt(options.PerformanceScheduleTimeStart)
              )
            .Build();

            IJobDetail KPIJob = JobBuilder.Create<KPIEvaluationJob>().Build();
            ITrigger KPITrigger = TriggerBuilder.Create()
            .WithIdentity("secondTrigger", "group1")
            .WithDailyTimeIntervalSchedule
              (s =>
                 s.WithIntervalInHours(24)
                .OnEveryDay()
                .StartingDailyAt(options.KPIEvalutionTimeStart)
              )
            .Build();

            IJobDetail routingJob = JobBuilder.Create<automaticRoutingJob>().Build();
            ITrigger routingTrigger = TriggerBuilder.Create()
            .WithIdentity("thirdTrigger", "group1")
            .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(options.AutomaticRoutingMinuteStart)
                .RepeatForever())
            .Build();

            scheduler.ScheduleJob(Job, Trigger); 
            scheduler.ScheduleJob(KPIJob,KPITrigger); 
            scheduler.ScheduleJob(routingJob, routingTrigger);
        }
    }
}