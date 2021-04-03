using MovieSocialNetworkApi.Models.Response;
using System;
using System.Collections.Generic;

namespace MovieSocialNetworkApi.Models
{
    public class ContentVM
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public SystemEntityVM Creator { get; set; }
        public ReactionVM ExistingReaction { get; set; }
        public List<ReactionStats> ReactionStats { get; set; }
    }
}
