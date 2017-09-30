using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loan.Domain;

namespace Loan.Repositories.IRepositories
{
    public interface ILoanRepository :IRepositoryBase<WCFLog>
    {
        IEnumerable<WCFLog> GetLogs(int page, int size, ref int total, List<Tuple<string, string>> sort);
    }
}
