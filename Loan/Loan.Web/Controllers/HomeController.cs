using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loan.Repositories.Repositories;
using Loan.Domain;
using Loan.Domain.RequestResponse;


namespace Loan.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetList()
        {
            var request = HttpContext;

            int rows = Request["rows"] != null ? int.Parse(Request["rows"].ToString()) : 10;
            int page = Request["page"] != null ? int.Parse(Request["page"].ToString()) : 1;

            LoanRepository lr = new LoanRepository(new DatabaseFactory());
            int count = 0;
            var list = lr.GetLogs(page, rows, ref count);

            var dataresult = new DataGridByPageResponse<WCFLog>(page, rows, count, list);


            return Json(dataresult, JsonRequestBehavior.AllowGet);
        }
    }
}