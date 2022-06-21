using UnityEngine;

namespace Jturesson.Advertisements
{
    public class ApplicationWrapper : IApplicationWrapper
    {
        public RuntimePlatform Platform => Application.platform;
    }
}