using System;

namespace MovieSocialNetworkApi.Exceptions
{
    public class BusinessException : Exception
    {
        public int Code { get; set; }
        public BusinessException()
        {
        }
        public BusinessException(string message)
            : base(message)
        {
        }

        public BusinessException(string message, int code)
            : base(message)
        {
            Code = code;
        }

        public BusinessException(string message, int code, Exception inner)
            : base(message, inner)
        {
            Code = code;
        }
    }
}
