using System;

namespace URLShort.Core.Exceptions;

public class DuplicateUrlException : Exception
{
    public DuplicateUrlException(string message) : base(message) { }
    public DuplicateUrlException(string message, Exception innerException) : base(message, innerException) { }


}
