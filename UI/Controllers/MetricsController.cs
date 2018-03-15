using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class MetricsController : Controller
    {
        // GET: Metrics
        public ActionResult incidentsByConsultant()
        {
            return View();
        }
    }
}