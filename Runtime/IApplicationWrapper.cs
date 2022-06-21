using UnityEngine;

namespace Jturesson.Advertisements
{
    public interface IApplicationWrapper
    {
        RuntimePlatform Platform { get; }
    }
}