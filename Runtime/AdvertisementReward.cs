using System.Threading.Tasks;
using UnityEngine.Advertisements;

namespace Jturesson.Advertisements
{
    public class AdvertisementReward : IAdvertisementReward, IUnityAdsLoadListener
    {
        private readonly IAdvertisementWrapper _advertisementWrapper;

        private readonly TaskCompletionSource<bool> _taskCompletionSource;
        private bool _isRunning;

        public AdvertisementReward(IAdvertisementWrapper advertisementWrapper)
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

            return _taskCompletionSource.Task;
        }
    }
}