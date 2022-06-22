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

        private TaskCompletionSource<bool> _taskCompletionSource;

        public AdvertisementInterstitial(IAdvertisementWrapper advertisementWrapper)
        {
            _advertisementWrapper = advertisementWrapper;
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            _taskCompletionSource.SetResult(true);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            _taskCompletionSource.SetResult(false);
        }

        public Task<bool> Load(string placementId)
        {
            if (_taskCompletionSource != null && _taskCompletionSource.Task.Status == TaskStatus.Running)
                return Task.FromResult(false);

            _taskCompletionSource = new TaskCompletionSource<bool>();

            _advertisementWrapper.Load(placementId, this);

            return _taskCompletionSource.Task;
        }
    }
}