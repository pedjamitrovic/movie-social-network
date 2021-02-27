using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MovieSocialNetworkApi.Models
{
    public class Paging
    {
        [DataMember(Name = "pageSize")]
        public int PageSize { get; set; } = 10;
        [DataMember(Name = "pageNumber")]
        public int PageNumber { get; set; } = 1;
    }
}
