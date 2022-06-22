using UnityEngine.Advertisements;

namespace Jturesson.Advertisements
{
    public class AdvertisementWrapper : IAdvertisementWrapper
    {
        public bool IsReady(string placementId) => Advertisement.IsReady(placementId);

        public void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad,
            IUnityAdsInitializationListener initializationListener) => Advertisement.Initialize(gameId,
            testMode, enablePerPlacementLoad, initializationListener);

        public void Load(string placementId, IUnityAdsLoadListener listener) =>
            Advertisement.Load(placementId, listener);

        public void Show(string placementId, IUnityAdsShowListener listener) =>
            Advertisement.Show(placementId, listener);

        public void BannerLoad(string placementId, BannerLoadOptions options) =>
            Advertisement.Banner.Load(placementId, options);

        public void BannerShow(string placementId, BannerOptions options) =>
            Advertisement.Banner.Show(placementId, options);

        public void BannerHide(bool destroy) =>
            Advertisement.Banner.Hide(destroy);

        public bool BannerIsLoaded => Advertisement.Banner.isLoaded;

        public void BannerSetPosition(BannerPosition position) =>
            Advertisement.Banner.SetPosition(position);

        public bool DebugMode => Advertisement.debugMode;
    }
}