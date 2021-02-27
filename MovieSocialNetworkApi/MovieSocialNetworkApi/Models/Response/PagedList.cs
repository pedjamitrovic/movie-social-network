using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Models
{
    public class PagedList<T>
    {
        public int? TotalCount { get; set; }
        public int? PageSize { get; set; }
        public int? Page { get; set; }
        public int? TotalPages { get; set; }
        public string SortOrder { get; set; }
        public string SortBy { get; set; }
        public List<T> Items { get; set; }
    }
}
