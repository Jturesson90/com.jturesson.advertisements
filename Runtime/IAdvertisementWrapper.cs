using UnityEngine.Advertisements;

namespace Jturesson.Advertisements
{
    public interface IAdvertisementWrapper
    {
        bool IsReady(string placementId);

        void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad,
            IUnityAdsInitializationListener initializationListener);

        void Load(string placementId, IUnityAdsLoadListener listener);
        void Show(string placementId, IUnityAdsShowListener listener);
        bool DebugMode { get; }

        void BannerLoad(string placementId, BannerLoadOptions options);
        void BannerShow(string placementId, BannerOptions options);
        void BannerHide(bool destroy = false);
        bool BannerIsLoaded { get; }
        void BannerSetPosition(BannerPosition position);
    }
}