using System.Collections;
using System.Linq.Expressions;

namespace LinqExtensions
{
    public static class WhereExtensions
    {
        public static IQueryable<T> WhereIfCollectionNotNullAndFilled<T, U>(this IQueryable<T> query, U? checkParam, Expression<Func<T, bool>> condition) where U : ICollection
        {
            var collection = checkParam as ICollection;
            return query.WhereIfTrue(collection! != null && collection!.Count > 0, condition);
        }

        public static IQueryable<T> WhereIfParamNotNull<T,U>(this IQueryable<T> query, U? checkParam, Expression<Func<T, bool>> condition)
        {
            bool paramIsvalid = false;

            if (checkParam != null)
            {
                if (typeof(U) == typeof(string) && string.IsNullOrEmpty(checkParam.ToString()))
                    paramIsvalid = false;
                else
                    paramIsvalid = true;
            }

            return query.WhereIfTrue(paramIsvalid, condition);
        }

        public static IQueryable<T> WhereIfTrue<T>(this IQueryable<T> query, bool useWhere, Expression<Func<T, bool>> condition)
        {
            return useWhere ? query.Where(condition) : query;
        }
    }
}
