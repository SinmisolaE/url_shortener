using System;

namespace URLShort.Core.Exceptions;

public class UrlTooLongException : Exception
{

    public UrlTooLongException(int max, int attempted) :
    base($"Url cannot exceed {max} characters, you sent {attempted} characters")
    {
        MaxLength = max;
        AttemptedLength = attempted;
    }
    public int MaxLength { get; }
    public int AttemptedLength { get; }

    
}
