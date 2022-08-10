using UnityEngine;

namespace JTuresson.Advertisements
{
    public interface IApplicationWrapper
    {
        RuntimePlatform Platform { get; }
    }
}