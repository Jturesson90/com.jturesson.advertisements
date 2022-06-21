using UnityEngine.Advertisements;

namespace Jturesson.Advertisements
{
    public class AdvertisementWrapper : IAdvertisementWrapper
    {
        public bool IsReady(string placementId) => Advertisement.IsReady(placementId);

        public void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad,
            IUnityAdsInitializationListener initializationListener) => Advertisement.Initialize(gameId,
            testMode, enablePerPlacementLoad, initializationListener);
    }
}