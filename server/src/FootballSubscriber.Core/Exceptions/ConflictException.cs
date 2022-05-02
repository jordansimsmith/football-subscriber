using System;

namespace FootballSubscriber.Core.Exceptions;

public class ConflictException : SystemException
{
    public ConflictException(string message) : base(message)
    {
    }
}