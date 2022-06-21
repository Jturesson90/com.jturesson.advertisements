using UnityEngine.Advertisements;

namespace Jturesson.Advertisements
{
    public interface IAdvertisementWrapper
    {
        bool IsReady(string placementId);

        void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad,
            IUnityAdsInitializationListener initializationListener);
    }
}