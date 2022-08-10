using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace JTuresson.Advertisements
{
    public class AdvertisementReward : IAdvertisementReward, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private readonly IAdvertisementWrapper _advertisementWrapper;

        private TaskCompletionSource<bool> _taskLoadCompletionSource;
        private TaskCompletionSource<RewardAdvertisementFinishedArgs> _taskShowCompletionSource;
        private bool _isRunning;
        private string _placementId;
        public event Action<bool> IsLoadedChanged;

        public AdvertisementReward(IAdvertisementWrapper advertisementWrapper)
        {
            _advertisementWrapper = advertisementWrapper;
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            _taskLoadCompletionSource.SetResult(true);
            _isRunning = false;
            OnIsLoadedChanged(true);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            _taskLoadCompletionSource.SetResult(false);
            _isRunning = false;

            OnIsLoadedChanged(false);
            if (error != UnityAdsLoadError.INITIALIZE_FAILED &&
                error != UnityAdsLoadError.INVALID_ARGUMENT)
            {
                Reload();
            }
        }

        public Task<bool> Load(string placementId)
        {
            if (_isRunning)
                return Task.FromResult(false);
            _isRunning = true;
            _taskLoadCompletionSource = new TaskCompletionSource<bool>();
            _advertisementWrapper.Load(placementId, this);
            _placementId = placementId;

            return _taskLoadCompletionSource.Task;
        }

        private void Reload()
        {
            if (_placementId != string.Empty)
            {
                _taskLoadCompletionSource = new TaskCompletionSource<bool>();
                _advertisementWrapper.Load(_placementId, this);
            }
        }

        public Task<RewardAdvertisementFinishedArgs> Show()
        {
            _taskShowCompletionSource = new TaskCompletionSource<RewardAdvertisementFinishedArgs>();
            _advertisementWrapper.Show(_placementId, this);

            return _taskShowCompletionSource.Task;
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogError(message);
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            OnIsLoadedChanged(false);
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId,
            UnityAdsShowCompletionState showCompletionState)
        {
            _taskShowCompletionSource.SetResult(
                new RewardAdvertisementFinishedArgs((RewardAdvertisementResult) showCompletionState)
            );
            Reload();
        }

        private void OnIsLoadedChanged(bool obj)
        {
            IsLoadedChanged?.Invoke(obj);
        }
    }
}