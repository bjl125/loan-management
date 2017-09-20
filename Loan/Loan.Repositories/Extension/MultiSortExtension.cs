using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> data, List<Tuple<string, string>> sortExpressions)
        {
            return (IQueryable<T>)OrderBy((IQueryable)data, sortExpressions);
        }
        public static IQueryable OrderBy(this IQueryable source, List<Tuple<string, string>> sortExpressions)
        {
            Type sourceType = source.ElementType;
            Expression queryExpr = source.Expression;
            ParameterExpression[] parameters = new ParameterExpression[] {
                Expression.Parameter(source.ElementType, "") };
            string methodAsc = "OrderBy";
            string methodDesc = "OrderByDescending";

            for (int i = 0; i < sortExpressions.Count; i++)
            {

                MemberInfo member = FindPropertyOrField(sourceType, sortExpressions[i].Item1, queryExpr == null);
                if (member == null)
                    throw new NullReferenceException();

                Expression memberExpre = member is PropertyInfo ?
                    Expression.Property(parameters[0], (PropertyInfo)member) :
                    Expression.Field(parameters[0], (FieldInfo)member);

                queryExpr = Expression.Call(
                    typeof(Queryable), "asc".Equals(sortExpressions[i].Item2, StringComparison.OrdinalIgnoreCase) ? methodAsc : methodDesc,
                    new Type[] { source.ElementType, memberExpre.Type },
                    queryExpr, Expression.Quote(Expression.Lambda(memberExpre, parameters)));
                methodAsc = "ThenBy";
                methodDesc = "ThenByDescending";


            }

            return source.Provider.CreateQuery(queryExpr);
            //return source;
        }
        static MemberInfo FindPropertyOrField(Type type, string memberName, bool staticAccess)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.DeclaredOnly |
                (staticAccess ? BindingFlags.Static : BindingFlags.Instance);
            foreach (Type t in SelfAndBaseTypes(type))
            {
                MemberInfo[] members = t.FindMembers(MemberTypes.Property | MemberTypes.Field,
                    flags, Type.FilterNameIgnoreCase, memberName);
                if (members.Length != 0) return members[0];
            }
            return null;
        }
        static IEnumerable<Type> SelfAndBaseTypes(Type type)
        {
            if (type.IsInterface)
            {
                List<Type> types = new List<Type>();
                AddInterface(types, type);
                return types;
            }
            return SelfAndBaseClasses(type);
        }
        static void AddInterface(List<Type> types, Type type)
        {
            if (!types.Contains(type))
            {
                types.Add(type);
                foreach (Type t in type.GetInterfaces()) AddInterface(types, t);
            }
        }
        static IEnumerable<Type> SelfAndBaseClasses(Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
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

        public static Expression<Func<TSource, Object>> GetExpression<TSource>(string propertyName)
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


        public static Expression<Func<TSource, TResult>> GetExpression<TSource, TResult>(string propertyName)
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
