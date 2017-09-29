using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Loan.Domain;
using Loan.Repositories.IRepositories;


namespace Loan.Repositories.Repositories
{
    public class LoanRepository : RepositoryBase<WCFLog, WCFLog>, ILoanRepository
    {
        public LoanRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<WCFLog> GetLogs(int page, int size, ref int total,List<Tuple<string,string>> sort)
        {
            Expression<Func<WCFLog, bool>> where = item => true;
            List<Tuple<string, string>> orderby = new List<Tuple<string, string>>();

            
            return this.GetManyByPage(page, size, ref total, where, sort);
        }
        
        public IEnumerable<WCFLog> GetOrderLogs(int page, int size, ref int total)
        {
            Expression<Func<WCFLog, bool>> where = item => true;
            List<Tuple<string, string>> orderby = new List<Tuple<string, string>>();

            orderby.Add(new Tuple<string, string>("ID", "asc"));
            orderby.Add(new Tuple<string, string>("Date", "Desc"));
            return this.GetManyByPage(page, size, ref total, where, orderby);
        }
    }
}
