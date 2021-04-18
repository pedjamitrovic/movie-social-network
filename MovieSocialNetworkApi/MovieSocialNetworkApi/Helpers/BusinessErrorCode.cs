namespace MovieSocialNetworkApi.Helpers
{
    public static class BusinessErrorCode
    {
        public const int UsernameAlreadyExists = 10000;
        public const int EmailAlreadyExists = 10001;
        public const int InvalidUsername = 10002;
        public const int InvalidPassword = 10003;
        public const int TitleAlreadyExists = 10004;
        public const int NotAdmin = 10005;
    }
}
