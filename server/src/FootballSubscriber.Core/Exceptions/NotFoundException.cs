using System;

namespace FootballSubscriber.Core.Exceptions;

public class NotFoundException : SystemException
{
    public NotFoundException(string message) : base(message) { }
}
