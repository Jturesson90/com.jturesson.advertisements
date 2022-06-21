using System;
using System.Threading.Tasks;

namespace Jturesson.Advertisements
{
    public interface IUnityAdsInitialization
    {
        Task<bool> Initialize();
    }
}