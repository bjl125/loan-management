using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loan.Web.Extension;
using Loan.Repositories.IRepositories;

namespace Loan.Web.Controllers
{
    public class BaseController : Controller
    {
        public ILoanRepository ILoanRepository { set; get; }

        /// <summary>
        /// 返回格式化日期的json
        /// </summary>
        /// <param name="data"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected JsonResult CustomJson(object data, JsonRequestBehavior behavior)
        {
            return new CustomJsonResult
            {
                Data = data,
                JsonRequestBehavior = behavior,
                JsonDateTimeFormateString = "yyyy-MM-dd HH:mm:ss"
            };
        }
        /// <summary>
        /// 返回格式化日期的json
        /// </summary>
        /// <param name="data"></param>
        /// <param name="behavior"></param>
        /// <param name="dateFormate">日期格式化字符串</param>
        /// <returns></returns>
        protected JsonResult CustomJson(object data, JsonRequestBehavior behavior,string dateFormate)
        {
            return new CustomJsonResult
            {
                Data = data,
                JsonRequestBehavior = behavior,
                JsonDateTimeFormateString = dateFormate
            };
        }
        /// <summary>
        /// 返回格式化日期的json
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected JsonResult CustomJson(object data, string contentType,System.Text.Encoding contentEncoding ,JsonRequestBehavior behavior)
        {
            return new CustomJsonResult
            {
                Data = data,
                JsonRequestBehavior = behavior,
                ContentEncoding=contentEncoding,
                ContentType=contentType,
                JsonDateTimeFormateString = "yyyy-MM-dd HH:mm:ss"
            };
        }
        /// <summary>
        /// 返回格式化日期的json
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <param name="dateFormate">日期格式化字符串</param>
        /// <returns></returns>
        protected JsonResult CustomJson(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior,string dateFormate)
        {
            return new CustomJsonResult
            {
                Data = data,
                JsonRequestBehavior = behavior,
                ContentEncoding = contentEncoding,
                ContentType = contentType,
                JsonDateTimeFormateString = dateFormate
            };
        }

    }
}