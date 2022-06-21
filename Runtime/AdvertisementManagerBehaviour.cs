using System;
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
        private bool _injected = false;

#if UNITY_EDITOR
        public void Inject(IAdvertisementInitializer advertisementInitializer,
            IUnityAdsInitialization unityAdsInitialization,
            IAdvertisementWrapper advertisementWrapper, IApplicationWrapper applicationWrapper)
        {
            _injected = true;

            _advertisementInitializer = advertisementInitializer;
            _unityAdsInitialization = unityAdsInitialization;
            _advertisementWrapper = advertisementWrapper;
            _applicationWrapper = applicationWrapper;
        }
#endif
        private void AutoInject()
        {
            _advertisementWrapper = new AdvertisementWrapper();
            _applicationWrapper = new ApplicationWrapper();
            _advertisementInitializer = advertisementInitializerSO;
        }

        private bool IsCurrentApplicationPlatform(AdvertisementPlatformSettings a) =>
            a.runtimePlatform == _applicationWrapper.Platform;


        public async Task<bool> Initialize()
        {
            if (!_injected) AutoInject();

            _platformSettings =
                _advertisementInitializer.AdvertisementPlatformSettings
                    .FirstOrDefault(IsCurrentApplicationPlatform);
            if (_platformSettings == null)
            {
                throw new NotSupportedException(
                    "Trying to initialize Advertisements for a runtime that is not supported. " +
                    _applicationWrapper.Platform);
            }

            _unityAdsInitialization =
                new UnityAdsInitialization(
                    _advertisementWrapper,
                    _platformSettings.gameId,
                    _advertisementInitializer.TestMode,
                    _advertisementInitializer.EnablePerPlacementLoad);

            var success = await _unityAdsInitialization.Initialize();


            return success;
        }


        public void ShowBanner()
        {
            throw new NotImplementedException();
        }

        public void HideBanner()
        {
            throw new NotImplementedException();
        }
    }
}