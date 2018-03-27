using Domain.DAL;
using Domain.Entities;
using log4net;
using Quartz;
using System;
using System.Linq;
using UI.Controllers;

namespace UI.Jobs
{
    public class performanceEvalutionJob : IJob
    {
        ILog logger = LogManager.GetLogger(typeof(MetricsController));
        private PlusBContext db = new PlusBContext();

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DateTime today = DateTime.Now;
                var shortDate = today.Date;

                var getConsultants = db.Consultants.Where(x => !x.ID.Equals(1));  // Get consultants 

                foreach (var i in getConsultants)   // Run a loop to extract id for every consultant
                {
                    var totalResolvedTickets = db.Tickets.Where(x => x.Id_Consultant == i.ID && x.ClosedDate == shortDate && x.Status == "Closed").Count();

                    var totalAssignedTickets = db.Tickets.Where(x=>x.AssignmentDate == shortDate && x.Id_Consultant == i.ID).Count();

                    var techWeightAvg = (from x in db.Tickets 
                                        join y in db.Technologies
                                        on x.Id_Technology equals y.ID
                                        where x.AssignmentDate == shortDate && x.Id_Consultant == i.ID
                                        select (int?)y.Weight).Sum() ?? 0;
                    techWeightAvg = techWeightAvg / totalAssignedTickets;

                    double? totalHoursTaken =  db.Tickets.Where(x =>x.ClosedDate == shortDate && x.Id_Consultant == i.ID).Select(x=>x.TotalResolutionHours).Sum();

                    double? perfAvg = totalResolvedTickets + totalAssignedTickets + techWeightAvg + totalHoursTaken;

                    try
                        {
                        var performanceEvalDetails = new PerformanceEvaluation
                        {
                            Date = shortDate,
                            idConsultant = i.ID,
                            TotalResolvedIncidents = totalResolvedTickets,
                            TotalAssignedIncidents = totalAssignedTickets,
                            TechWeightAverage = techWeightAvg,
                            TotalHoursTaken = Math.Round(totalHoursTaken.Value, 2),
                            PerformanceAverage = Math.Round(perfAvg.Value, 2)
                            };

                            using (var x = new PlusBContext())
                            {
                                x.PerformanceEvalutions.Add(performanceEvalDetails);
                                x.SaveChanges();
                            }
                        }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                    }                    
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

        }
    }
}