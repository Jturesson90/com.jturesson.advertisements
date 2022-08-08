using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Jturesson.Advertisements
{
    public class AdvertisementReward : IAdvertisementReward, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private readonly IAdvertisementWrapper _advertisementWrapper;

        private readonly TaskCompletionSource<bool> _taskLoadCompletionSource;
        private readonly TaskCompletionSource<RewardAdvertisementFinishedArgs> _taskShowCompletionSource;
        private bool _isRunning;
        private string _placementId;

        public AdvertisementReward(IAdvertisementWrapper advertisementWrapper)
        {
            _advertisementWrapper = advertisementWrapper;
            _taskLoadCompletionSource = new TaskCompletionSource<bool>();
            _taskShowCompletionSource = new TaskCompletionSource<RewardAdvertisementFinishedArgs>();
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            _taskLoadCompletionSource.SetResult(true);
            _isRunning = false;
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            _taskLoadCompletionSource.SetResult(false);
            _isRunning = false;
        }

        public Task<bool> Load(string placementId)
        {
            if (_isRunning)
                return Task.FromResult(false);
            _isRunning = true;
            _advertisementWrapper.Load(placementId, this);
            _placementId = placementId;
            return _taskLoadCompletionSource.Task;
        }

        public Task<RewardAdvertisementFinishedArgs> Show()
        {
            _advertisementWrapper.Show(_placementId, this);
            return _taskShowCompletionSource.Task;
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogError(message);
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId,
            UnityAdsShowCompletionState showCompletionState)
        {
            _taskShowCompletionSource.SetResult(
                new RewardAdvertisementFinishedArgs((RewardAdvertisementResult) showCompletionState,
                    "")
            );
        }
    }
}