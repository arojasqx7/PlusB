using Quartz;
using Quartz.Impl;
using UI.Jobs;

namespace UI.Background_Jobs
{
    public class SingletonJob
    {
        protected static SingletonJob _obj;

        private SingletonJob()
        { 
        }

        public static SingletonJob GetObject()
        {
            if (_obj == null)
            {
               _obj = new SingletonJob();
            }
            return _obj;
        }

        public void GetAutomaticRoutingJob()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail routingJob2 = JobBuilder.Create<automaticRoutingJob>().Build();

            ITrigger routingTrigger2 = TriggerBuilder.Create()
            .WithIdentity("routeTrigger", "group1")
            .StartNow()
            .Build();

             scheduler.ScheduleJob(routingJob2, routingTrigger2);
        }

        public void GetPerformanceEvaluationJob()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail PerfEvalJob = JobBuilder.Create<performanceEvalutionJob>().Build();
            ITrigger PerfEvalTrigger = TriggerBuilder.Create()
            .WithIdentity("perfEvalTrigger", "group1")
            .StartNow()
            .Build();

            scheduler.ScheduleJob(PerfEvalJob, PerfEvalTrigger);
        }

        public void GetKPIEvaluationJob()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail KPIJob = JobBuilder.Create<KPIEvaluationJob>().Build();
            ITrigger KPITrigger = TriggerBuilder.Create()
            .WithIdentity("KPITrigger", "group1")
            .StartNow()
            .Build();

            scheduler.ScheduleJob(KPIJob, KPITrigger);
        }
    }
}