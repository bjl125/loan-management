using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Domain
{
    public interface IDatabaseFactory : IDisposable
    {
        DBEntities Create();
    }
}
