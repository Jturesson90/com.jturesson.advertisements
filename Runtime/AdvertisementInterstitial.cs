using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Advertisements;

namespace Jturesson.Advertisements
{
    public class AdvertisementInterstitial : IUnityAdsLoadListener, IAdvertisementInterstitial
    {
        private readonly IAdvertisementWrapper _advertisementWrapper;

        private readonly TaskCompletionSource<bool> _taskCompletionSource;
        private bool _isRunning;
        private string _placementId;

        public AdvertisementInterstitial(IAdvertisementWrapper advertisementWrapper)
        {
            _advertisementWrapper = advertisementWrapper;
            _taskCompletionSource = new TaskCompletionSource<bool>();
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            _taskCompletionSource.SetResult(true);
            _isRunning = false;
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            _taskCompletionSource.SetResult(false);
            _isRunning = false;
        }

        public Task<bool> Load(string placementId)
        {
            if (_isRunning)
                return Task.FromResult(false);
            _isRunning = true;
            _advertisementWrapper.Load(placementId, this);
            _placementId = placementId;

            return _taskCompletionSource.Task;
        }

        public void Show()
        {
            _advertisementWrapper.Show(_placementId, null);
        }
    }
}