using System.Collections.Generic;
using Yocale.eShop.Utility.Exception;

namespace Yocale.eShop.Utility.Data
{
    public static class Extensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> items, int totalRecordCount, int totalPages, int pageSize = 25, int pageNumber = 1)
        {
            return PagedList<T>.Create(items, totalRecordCount,totalPages, pageSize, pageNumber);
        }

        public static ResultModel<T> ToResultModel<T>(this T obj)
        {
            return ResultModel<T>.Create(obj);
        }

        public static int GetCode(this Error obj)
        {
            return obj.Code;
        }
    }
}
