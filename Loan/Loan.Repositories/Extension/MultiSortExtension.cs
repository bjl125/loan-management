using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Repositories.Extension
{
    public static class MultiSortExtension
    {
        /// <summary>
        /// 通过列名称及排序规则实现多列排序
        /// 如果输入的列不存在抛出异常
        /// 如果列多次出现，只第一个有效
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="sortExpressions">
        /// 排序列列表，第一个是列名称，第二个是排序规则（asc/desc)
        /// </param>
        /// <returns></returns>
        public static IEnumerable<T> MultipleSort<T>(this IEnumerable<T> data, List<Tuple<string, string>> sortExpressions)
        {
            if (sortExpressions == null || sortExpressions.Count < 0)
            {
                return data;
            }

            IEnumerable<T> query = from item in data select item;
            IOrderedEnumerable<T> orderedQuery = null;

            for (int i = 0; i < sortExpressions.Count; i++)
            {
                int index = i;
                Func<T, object> expression = item => item.GetType().GetProperty(sortExpressions[index].Item1).GetValue(item, null);

                if (sortExpressions[index].Item2 == "asc")
                {
                    orderedQuery = (index == 0) ? query.OrderBy(expression) : orderedQuery.ThenBy(expression);
                }
                else
                {
                    orderedQuery = (index == 0) ? query.OrderByDescending(expression) : orderedQuery.ThenByDescending(expression);
                }
            }

            query = orderedQuery;

            return query;
        }

        /// <summary>
        /// 通过列名称及排序规则实现多列排序
        /// 如果输入的列不存在抛出异常
        /// 如果列多次出现，只第一个有效
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="sortExpressions">
        /// 排序列列表，第一个是列名称，第二个是排序规则（asc/desc)
        /// </param>
        /// <returns></returns>
        public static IQueryable<T> MultipleSort<T>(this IQueryable<T> data, List<Tuple<string, string>> sortExpressions)
        {
            if (sortExpressions == null || sortExpressions.Count < 0)
            {
                return data;
            }

            IQueryable<T> query = from item in data select item;
            IOrderedQueryable<T> orderedQuery = null;

            for (int i = 0; i < sortExpressions.Count; i++)
            {
                int index = i;
                Func<T, object> exps = item => item.GetType().GetProperty(sortExpressions[index].Item1).GetValue(item, null);
                //Expression<Func<T, object>> exp = item => item.GetType().GetProperty(sortExpressions[index].Item1).GetValue(item, null);

                //Expression<Func<T, object>> exp = item => item.GetType().InvokeMember(sortExpressions[index].Item1, System.Reflection.BindingFlags.GetProperty, null, item, null);
                Type propertyType = typeof(T).GetProperty(sortExpressions[index].Item1).PropertyType;
                var exp = GetExpression<T>(sortExpressions[index].Item1);

                if (sortExpressions[index].Item2 == "asc")
                {
                    orderedQuery = (index == 0) ? query.OrderBy(exp) : orderedQuery.ThenBy(exp);
                }
                else
                {
                    orderedQuery = (index == 0) ? query.OrderByDescending(exp) : orderedQuery.ThenByDescending(exp);
                }
            }

            query = orderedQuery;

            return query;
        }

        public static Expression<Func<TSource,Object>> GetExpression<TSource>(string propertyName)
        {
            // x=>x.name=="dd"
            //x形如上
            Type propertyType = typeof(TSource).GetProperty(propertyName).PropertyType;

            var funcType = typeof(Func<,>).MakeGenericType(typeof(TSource), propertyType);

            var param = Expression.Parameter(typeof(TSource), "x");

            Expression conversion = param;
            conversion = Expression.Property(param, propertyName);

            if (!conversion.Type.IsValueType)
            {
                var converted = Expression.Convert(conversion, typeof(Object));
                return Expression.Lambda<Func<TSource, Object>>(converted, param);


            }
            else
            {
                return Expression.Lambda<Func<TSource, Object>>(conversion, param);
            }

            //return Expression.Lambda(funcType, conversion, param);
            //Expression.Convert(Expression.Property(param, propertyName), propertyType);
            //return Expression.Lambda<Func<TSource, object>>(conversion, param);

        }


        public static Expression<Func<TSource, TResult>> GetExpression<TSource,TResult>(string propertyName)
        {
            // x=>x.name=="dd"
            //x形如上
            Type propertyType = typeof(TSource).GetProperty(propertyName).PropertyType;

            var funcType = typeof(Func<,>).MakeGenericType(typeof(TSource), propertyType);

            var param = Expression.Parameter(typeof(TSource), "x");

            Expression conversion = param;
            conversion = Expression.Property(param, propertyName);

            if (conversion.Type.IsValueType)
            {
                var converted = Expression.Convert(conversion, typeof(TResult));
                return Expression.Lambda<Func<TSource, TResult>>(converted, param);


            }
            else
            {
                return Expression.Lambda<Func<TSource, TResult>>(conversion, param);
            }

            //return Expression.Lambda(funcType, conversion, param);
            //Expression.Convert(Expression.Property(param, propertyName), propertyType);
            //return Expression.Lambda<Func<TSource, object>>(conversion, param);

        }
    }
}
