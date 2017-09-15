using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

using Loan.Domain;
using System.Data;
using System.Data.SqlClient;
using Loan.Repositories.Extension;

namespace Loan.Repositories
{
    public abstract class RepositoryBase<T, S> : IRepositoryBase<T> where T : class
    {
        private DBEntities dbContext;

        private IDbSet<T> dbset;
        /// <summary>
        /// 工厂方法
        /// </summary>
        protected IDatabaseFactory DatabaseFactory { private set; get; }

        public RepositoryBase(IDatabaseFactory dbFactory)
        {
            this.DatabaseFactory = dbFactory;
            this.dbset = DBContext.Set<T>();

        }

        protected DBEntities DBContext
        {
            get
            {
                return dbContext ?? (dbContext = DatabaseFactory.Create());
            }
        }
        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            var dels = dbset.Where(where);
            foreach (var entity in dels)
            {
                dbset.Remove(entity);
            }
        }

        public void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).FirstOrDefault<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return dbset;
        }

        public T GetById(long Id)
        {
            return dbset.Find(Id);
        }

        public T GetById(string Id)
        {
            return dbset.Find(Id);
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).ToList();
        }

        public void Update(T entity)
        {
            dbset.Attach(entity);
            DBContext.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<TT> ExecuteSqlQuery<TT>(string sql)
        {
            return DBContext.Database.SqlQuery<TT>(sql);
        }

        public IEnumerable<TT> ExecuteSqlQuery<TT>(string sql, params object[] commandParameters)
        {
            return DBContext.Database.SqlQuery<TT>(sql, commandParameters);
        }

        public TT ExecuteT<TT>(string sql)
        {
            return DBContext.Database.SqlQuery<TT>(sql).FirstOrDefault();
        }

        public IEnumerable<TT> ExecuteStoreProcedure<TT>(string procedureName, params object[] commandParameters)
        {
            return DBContext.Database.SqlQuery<TT>(procedureName, commandParameters);
        }

        public int ExecuteNonQuerySql(string sql, params object[] commandParameters)
        {
            return DBContext.Database.ExecuteSqlCommand(sql, commandParameters);
        }


        public TT ExecuteT<TT>(string sql, params object[] commandParameters)
        {
            return DBContext.Database.SqlQuery<TT>(sql, commandParameters).FirstOrDefault<TT>();
        }

        public void ExecuteNonQuery(string sql)
        {
            DBContext.Database.ExecuteSqlCommand(sql);
        }

        public void ExecuteNonQuery(string sql, params object[] commandParameters)
        {
            DBContext.Database.ExecuteSqlCommand(sql, commandParameters);
            //AliPayContext.Database.SqlQuery<DataSet>(sql, commandParameters);
        }


        public IEnumerable<T> GetManyByPage(int pageindex, int pagesize, ref int totalCount, Expression<Func<T, bool>> where)
        {
            totalCount = dbset.Where(where).Count();
            return dbset.Where(where).Skip((pageindex - 1) * pagesize).Take(pagesize);
        }

        public IEnumerable<T> GetManyByPage<TKey>(int pageindex, int pagesize, ref int totalCount, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, bool isAsc)
        {
            totalCount = dbset.Where(where).Count();
            if (isAsc)
                return dbset.Where(where).OrderBy(order).Skip((pageindex - 1) * pagesize).Take(pagesize);
            else
                return dbset.Where(where).OrderByDescending(order).Skip((pageindex - 1) * pagesize).Take(pagesize);
        }

        public IEnumerable<T> GetManyByPage(int pageindex, int pagesize, ref int totalCount, Expression<Func<T, bool>> where, List<Tuple<string, string>> sortExpressions)
        {
            totalCount = dbset.Where(where).Count();
            return dbset.Where(where).MultipleSort(sortExpressions).Skip((pageindex - 1) * pagesize).Take(pagesize);
        }
    }
}
