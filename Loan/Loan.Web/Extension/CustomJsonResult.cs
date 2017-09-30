using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Loan.Web.Extension
{
    public class CustomJsonResult : JsonResult
    {
        private string _formateStr;
        public string JsonDateTimeFormateString
        {
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                    this._formateStr = value;
                else
                    this._formateStr = "yyyy-MM-dd HH:mm:ss";
            }
            get
            {
                if (!String.IsNullOrWhiteSpace(_formateStr))
                    return _formateStr;
                else
                    return "yyyy-MM-dd HH:mm:ss";
            }
        }
        public override void ExecuteResult(ControllerContext context)
        {
            //base.ExecuteResult(context);

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if ((this.JsonRequestBehavior == System.Web.Mvc.JsonRequestBehavior.DenyGet) && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("GetNotAllowed");
            }
            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                var dateConverter = new IsoDateTimeConverter() { DateTimeFormat = this.JsonDateTimeFormateString };
                response.Write(JsonConvert.SerializeObject(this.Data, Formatting.Indented, dateConverter));
            }
        }
    }
}