using System.Collections.Generic;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Helpers;

public class ApiEntityComparer : IEqualityComparer<IApiEntity>
{
    public bool Equals(IApiEntity x, IApiEntity y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (ReferenceEquals(x, null))
            return false;
        if (ReferenceEquals(y, null))
            return false;
        if (x.GetType() != y.GetType())
            return false;
        return x.ApiId == y.ApiId;
    }

    public int GetHashCode(IApiEntity obj)
    {
        return obj.ApiId.GetHashCode();
    }
}
