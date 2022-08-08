using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Jturesson.Advertisements
{
    public class AdvertisementManagerBehaviour : MonoBehaviour, IAdvertisementManager
    {
        public AdvertisementInitializerSO advertisementInitializerSO;

        private IAdvertisementInitializer _advertisementInitializer;
        private IUnityAdsInitialization _unityAdsInitialization;
        private IAdvertisementWrapper _advertisementWrapper;
        private IApplicationWrapper _applicationWrapper;
        private AdvertisementPlatformSettings _platformSettings;

        private IAdvertisementBanner _advertisementBanner;
        private IAdvertisementInterstitial _advertisementInterstitial;
        private IAdvertisementReward _advertisementReward;

        private bool _injected = false;


#if UNITY_EDITOR
        public void Inject(IAdvertisementInitializer advertisementInitializer,
            IUnityAdsInitialization unityAdsInitialization,
            IAdvertisementWrapper advertisementWrapper, IApplicationWrapper applicationWrapper,
            IAdvertisementInterstitial advertisementInterstitial,
            IAdvertisementBanner advertisementBanner,
            IAdvertisementReward advertisementReward
        )
        {
            _injected = true;

            _advertisementInitializer = advertisementInitializer;
            _unityAdsInitialization = unityAdsInitialization;
            _advertisementWrapper = advertisementWrapper;
            _applicationWrapper = applicationWrapper;
            _advertisementInterstitial = advertisementInterstitial;
            _advertisementBanner = advertisementBanner;
            _advertisementReward = advertisementReward;
        }
#endif
        private void Setup()
        {
            _advertisementWrapper = new AdvertisementWrapper();
            _applicationWrapper = new ApplicationWrapper();
            _advertisementInitializer = advertisementInitializerSO;
            _unityAdsInitialization = new UnityAdsInitialization(_advertisementWrapper);
            _advertisementInterstitial =
                new AdvertisementInterstitial(_advertisementWrapper);
            _advertisementBanner = new AdvertisementBanner(_advertisementWrapper);
            _advertisementReward = new AdvertisementReward(_advertisementWrapper);
        }

        private bool IsCurrentApplicationPlatform(AdvertisementPlatformSettings a) =>
            a.runtimePlatform == _applicationWrapper.Platform;

        public async Task<bool> Initialize()
        {
            if (!_injected) Setup();

            _platformSettings =
                _advertisementInitializer.AdvertisementPlatformSettings
                    .FirstOrDefault(IsCurrentApplicationPlatform);
            if (_platformSettings == null)
            {
                throw new NotSupportedException(
                    $"Trying to initialize Advertisements for a runtime that is not supported. {_applicationWrapper.Platform}");
            }

            var success = await _unityAdsInitialization.Initialize(_platformSettings.gameId,
                _advertisementInitializer.TestMode, _advertisementInitializer.EnablePerPlacementLoad);
            if (!success) return false;

            var li = new List<Task<bool>>();
            var banner = _platformSettings.banner;
            if (banner.enabled)
            {
                li.Add(_advertisementBanner.Load(banner.placementId, banner.bannerPosition));
            }

            var interstitial = _platformSettings.interstitial;
            if (interstitial.enabled)
            {
                li.Add(_advertisementBanner.Load(interstitial.placementId));
            }

            var reward = _platformSettings.reward;
            if (reward.enabled)
            {
                li.Add(_advertisementBanner.Load(reward.placementId));
            }


            var t = Task.WhenAll(li.ToArray());
            await t;

            return t.Result.All(a => a);
        }


        public void ShowBanner()
        {
            _advertisementBanner.Show();
        }

        public void HideBanner()
        {
            _advertisementBanner.Hide();
        }

        public bool BannerIsReadyAndEnabled()
        {
            return _advertisementBanner.IsLoaded();
        }

        public bool InterstitialIsReadyAndEnabled()
        {
            return _platformSettings.interstitial.enabled &&
                   _advertisementWrapper.IsReady(_platformSettings.interstitial.placementId);
        }

        public void ShowInterstitial()
        {
            _advertisementInterstitial.Show();
        }

        public bool RewardIsReadyAndEnabled()
        {
            return _platformSettings.reward.enabled &&
                   _advertisementWrapper.IsReady(_platformSettings.reward.placementId);
        }

        public Task<RewardAdvertisementFinishedArgs> ShowReward()
        {
            throw new NotImplementedException();
        }


        public event Action<bool> RewardIsReadyChanged;
    }
}