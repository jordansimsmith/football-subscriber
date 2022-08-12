using System;

namespace FootballSubscriber.Core.Exceptions;

public class InternalServerErrorException : SystemException
{
    public InternalServerErrorException(string message) : base(message) { }
}
