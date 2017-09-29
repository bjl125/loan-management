using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Loan.Repositories.Repositories;
using Loan.Domain;
using System.Collections.Generic;

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
                List<Tuple<string, string>> sort = new List<Tuple<string, string>>();
                sort.Add(new Tuple<string, string>("ID", "desc"));
                var list = lr.GetLogs(1, 5, ref s, sort);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod]
        public void TestQueryOrderByPage()
        {
            try
            {
                LoanRepository lr = new LoanRepository(new DatabaseFactory());
                int s = 0;
                var list = lr.GetOrderLogs(1, 5, ref s);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
