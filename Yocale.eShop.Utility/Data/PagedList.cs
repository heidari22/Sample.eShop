using System;
using System.Collections.Generic;

namespace Yocale.eShop.Utility.Data
{
    public sealed class PagedList<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalRecordCount { get; set; }
        public int TotalPageCount { get
            {
                int reminder = 0;

                var result = Math.DivRem(TotalRecordCount, PageSize, out reminder);
                if (reminder > 0)
                {
                    result++;
                }

                return result;
            }
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public static PagedList<T> Create(IEnumerable<T> items, int totalRecordCount, int totalPages, int pageSize, int pageNumber)
        {
            return new PagedList<T>
            {
                Items = items,
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalRecordCount = totalRecordCount,
            };
        }
    }
}
