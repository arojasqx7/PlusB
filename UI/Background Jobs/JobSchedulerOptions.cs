using Quartz;

namespace UI.Background_Jobs
{
    public class JobSchedulerOptions
    {
        public JobSchedulerOptions(TimeOfDay performance, TimeOfDay kpi, int routing)
        {
            this.PerformanceScheduleTimeStart = performance;
            this.KPIEvalutionTimeStart = kpi;
            this.AutomaticRoutingMinuteStart = routing;
        }

        public JobSchedulerOptions()
            :this(TimeOfDay.HourAndMinuteOfDay(23, 55), TimeOfDay.HourAndMinuteOfDay(23, 56), 20)
        { }

        public TimeOfDay PerformanceScheduleTimeStart { get; set; }
        public TimeOfDay KPIEvalutionTimeStart { get; set; }
        public int AutomaticRoutingMinuteStart { get; set; }
    }
}