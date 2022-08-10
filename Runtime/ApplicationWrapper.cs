using UnityEngine;

namespace JTuresson.Advertisements
{
    public class ApplicationWrapper : IApplicationWrapper
    {
        public RuntimePlatform Platform => Application.platform;
    }
}