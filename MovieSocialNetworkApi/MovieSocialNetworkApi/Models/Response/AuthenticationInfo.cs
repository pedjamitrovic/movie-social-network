using System;

namespace MovieSocialNetworkApi.Models
{
    public class AuthenticationInfo
    {
        public int Id { get; set; }
        public string QualifiedName { get; set; }
        public string Discriminator { get; set; }
        public bool IsBanned { get; set; }
        public DateTimeOffset BannedUntil { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
