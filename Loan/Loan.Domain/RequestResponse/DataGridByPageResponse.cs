using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Domain.RequestResponse
{
    public class DataGridByPageResponse<T>
    {
        /// <summary>
        /// 第几页
        /// </summary>
        public int page { set; get; }
        /// <summary>
        /// 共几页
        /// </summary>
        public int total { set; get; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int records { set; get; }
        /// <summary>
        /// 当前页记录内容
        /// </summary>
        public IEnumerable<T> rows { set; get; }


        public DataGridByPageResponse(int page, int pagesize, int count, IEnumerable<T> rows)
        {
            this.page = page;
            this.records = count;
            this.rows = rows;
            this.total = (int)(Math.Ceiling((double)count / pagesize));
        }
    }
}
