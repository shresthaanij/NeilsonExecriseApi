using System;
using System.Collections.Generic;
using System.Linq;

namespace NeilsonAssessment.Api.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PerPage { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int page, int perPage)
        {
            TotalCount = count;
            PerPage = perPage;
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(count / (double)perPage);

            AddRange(items);
        }

        public static PagedList<T> Create(IQueryable<T> source, int page, int perPage)
        {
            var count = source.Count();
            var items = source.Skip((page - 1) * perPage).Take(perPage).ToList();

            return new PagedList<T>(items, count, page, perPage);
        }
    }
}
