using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Loan.Repositories.Extension;

namespace Loan.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);

        T GetById(long Id);

        T GetById(string Id);

        T Get(Expression<Func<T, bool>> where);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

        IEnumerable<T> GetManyByPage(int pageindex, int pagesize, ref int totalCount, Expression<Func<T, bool>> where);
        IEnumerable<T> GetManyByPage<TKey>(int pageindex, int pagesize,ref int totalCount, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, bool isAsc);
        IEnumerable<T> GetManyByPage(int pageindex, int pagesize,ref int totalCount, Expression<Func<T, bool>> where, List<Tuple<string, string>> sortExpressions);

        IEnumerable<T> GetManyByPage(int pageindex, int pagesize, ref int totalCount, Expression<Func<T, bool>> where, string ordering);

        #region 扩展

        IEnumerable<TT> ExecuteSqlQuery<TT>(string sql);
        IEnumerable<TT> ExecuteSqlQuery<TT>(string sql, params object[] commandParameters);

        TT ExecuteT<TT>(string sql);
        TT ExecuteT<TT>(string sql, params object[] commandParameters);

        IEnumerable<TT> ExecuteStoreProcedure<TT>(string procedureName, params object[] commandParameters);
        int ExecuteNonQuerySql(string sql, params object[] commandParameters);
        void ExecuteNonQuery(string sql);
        void ExecuteNonQuery(string sql, params object[] commandParameters);
        #endregion


    }
}
