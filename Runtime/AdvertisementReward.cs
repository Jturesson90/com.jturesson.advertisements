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
        private string _placementId;
        public event Action<bool> IsLoadedChanged;

        public AdvertisementReward(IAdvertisementWrapper advertisementWrapper)
        {
            _advertisementWrapper = advertisementWrapper;
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"AdvertisementReward - OnUnityAdsLoaded({placementId}) ");
            _taskLoadCompletionSource.SetResult(true);
            OnIsLoadedChanged(true);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"AdvertisementReward - OnUnityAdsFailedToLoad({placementId}) " + message);
            _taskLoadCompletionSource.SetResult(false);

            OnIsLoadedChanged(false);
            if (error != UnityAdsLoadError.INITIALIZE_FAILED &&
                error != UnityAdsLoadError.INVALID_ARGUMENT)
            {
                Reload();
            }
        }

        private bool IsRunning()
        {
            return _taskLoadCompletionSource != null &&
                   _taskLoadCompletionSource.Task.Status == TaskStatus.Running;
        }

        public Task<bool> Load(string placementId)
        {
            Debug.Log($"AdvertisementReward - Load({placementId})");
            if (IsRunning())
                return Task.FromResult(false);
            _taskLoadCompletionSource = new TaskCompletionSource<bool>();
            _advertisementWrapper.Load(placementId, this);
            Debug.Log($"AdvertisementReward - Load({placementId}) setting _placementId");
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
            Debug.Log($"AdvertisementReward - Show({_placementId}) ");
            _taskShowCompletionSource = new TaskCompletionSource<RewardAdvertisementFinishedArgs>();
            _advertisementWrapper.Show(_placementId, this);

            return _taskShowCompletionSource.Task;
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogError(
                $"AdvertisementReward - OnUnityAdsShowFailure {placementId} {error} {message} ");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"AdvertisementReward - OnUnityAdsShowStart - {placementId}");
            OnIsLoadedChanged(false);
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"AdvertisementReward - OnUnityAdsShowClick - {placementId}");
        }

        public void OnUnityAdsShowComplete(string placementId,
            UnityAdsShowCompletionState showCompletionState)
        {
            Debug.Log(
                $"AdvertisementReward - OnUnityAdsShowComplete - {placementId} UnityAdsShowCompletionState.{showCompletionState}");
            _taskShowCompletionSource.SetResult(
                new RewardAdvertisementFinishedArgs((RewardAdvertisementResult) showCompletionState)
            );
            Reload();
        }

        private void OnIsLoadedChanged(bool obj)
        {
            Debug.Log($"AdvertisementReward - OnIsLoadedChanged - IsLoaded = {obj}");
            IsLoadedChanged?.Invoke(obj);
        }
    }
}