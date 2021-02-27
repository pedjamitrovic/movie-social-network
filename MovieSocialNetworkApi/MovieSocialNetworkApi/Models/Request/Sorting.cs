using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using static MovieSocialNetworkApi.Helpers.SortOrder;

namespace MovieSocialNetworkApi.Models
{
    public class Sorting
    {
        [DataMember(Name = "sortOrder")]
        public string SortOrder { get; set; } = Desc;
        [DataMember(Name = "sortBy")]
        public string SortBy { get; set; }
    }
}
