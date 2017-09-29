using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loan.Repositories.IRepositories;
using Loan.Domain;
using Loan.Domain.RequestResponse;


namespace Loan.Web.Controllers
{
    public class HomeController : Controller
    {
        public ILoanRepository ILoanRepository { set; get; }
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
            string sidx = Request["sidx"] != null ? Request["sidx"].ToString() : "";
            string sord = Request["sord"] != null ? Request["sord"].ToString() : "";

            
            int count = 0;
            List<Tuple<string, string>> sort = new List<Tuple<string, string>>();
            if (!string.IsNullOrWhiteSpace(sidx) && !string.IsNullOrWhiteSpace(sord))
                sort.Add(new Tuple<string, string>(sidx,sord));
            var list = ILoanRepository.GetLogs(page, rows, ref count, sort);

            var dataresult = new DataGridByPageResponse<WCFLog>(page, rows, count, list);


            return Json(dataresult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveInfo(WCFLog model)
        {


            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}