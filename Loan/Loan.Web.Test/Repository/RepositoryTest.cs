using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Loan.Repositories.Repositories;
using Loan.Domain;


namespace Loan.Web.Test.Repository
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void TestQueryByPage()
        {
            try
            {
                LoanRepository lr = new LoanRepository(new DatabaseFactory());
                int s = 0;
                var list = lr.GetLogs(1, 5, ref s);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
