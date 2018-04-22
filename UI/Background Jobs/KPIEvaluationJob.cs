using Domain.DAL;
using Domain.Entities;
using log4net;
using Quartz;
using System;
using System.Linq;
using UI.Controllers;

namespace UI.Jobs
{
    public class KPIEvaluationJob : IJob
    {
        ILog logger = LogManager.GetLogger(typeof(MetricsController));
        private PlusBContext db = new PlusBContext();

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DateTime today = DateTime.Now;
                var shortDate = today.Date;

                var getKPIData = from a in db.Tickets
                                 join b in db.CustomerSLAS
                                 on a.Id_Customer equals b.IdCustomer
                                 join c in db.SLAS
                                 on b.IdSLA equals c.ID
                                 join d in db.ConsultantsKPIs
                                 on a.Id_Consultant equals d.idConsultant
                                 join e in db.KPIS
                                 on d.idKPI equals e.ID
                                 where a.Status == "Closed" && a.ClosedDate == shortDate
                                 select new { a.Id, a.Id_Consultant, e.ID, Score = ((e.FormulaValue + a.TotalResolutionHours) / c.ResolutionTimeAverage) };

                    foreach (var data in getKPIData)   // Run a loop to extract id for every consultant
                    {
                        string range;
                        if (data.Score > 0 && data.Score <= 10)
                        {
                            range = "Under average";   
                        }
                        else if (data.Score > 10 && data.Score <= 25)
                        {
                            range = "Good";
                        }
                        else
                        {
                            range = "Exceed Expectation";
                        }
                        try
                        {
                            var KPIEvalDetails = new KPIEvaluation
                            {
                                Date = shortDate,
                                idConsultant = data.Id_Consultant,
                                idKPI = data.ID,
                                Score = Convert.ToDouble(data.Score),
                                Range = range, 
                                idTicket = data.Id
                            };
                            using (var x = new PlusBContext())
                            {
                                x.KPIEvaluations.Add(KPIEvalDetails);
                                x.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.ToString());
                        }
              } // end getConsultants foreach
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }
    }
}