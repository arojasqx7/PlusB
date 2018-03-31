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

            IJobDetail Job = JobBuilder.Create<performanceEvalutionJob>().Build();

            ITrigger Trigger = TriggerBuilder.Create()
            .WithIdentity("firstTrigger", "group1")
            //.StartNow().WithSimpleSchedule(x => x.WithIntervalInSeconds(15).RepeatForever())
            .WithDailyTimeIntervalSchedule
              (s =>
                 s.WithIntervalInHours(24)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(12, 43))
              )
            .Build();

            scheduler.ScheduleJob(Job, Trigger);
        }
    }
}