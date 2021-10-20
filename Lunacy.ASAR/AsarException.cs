using System;

namespace Lunacy.ASAR
{
    public class AsarException : Exception
    {
        public AsarException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}