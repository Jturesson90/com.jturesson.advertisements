using System;
using System.Threading.Tasks;

namespace Jturesson.Advertisements
{
    public interface IUnityAdsInitialization
    {
        Task<bool> Initialize(string gameId, bool testMode = false,
            bool enablePerPlacementLoad = false);
    }
}