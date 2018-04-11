using Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class ReportsController : Controller
    {
        public void exportToCSV(ICollection listObject)
        {
            var sb = new StringBuilder();

            foreach(var item in listObject)
            {

            }
        }
    }
}