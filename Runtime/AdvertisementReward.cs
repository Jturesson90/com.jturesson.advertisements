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
        private bool _isLoaded;

        public bool IsLoaded
        {
            get => _isLoaded;
            private set
            {
                if (_isLoaded == value) return;
                _isLoaded = value;
                IsLoadedChanged?.Invoke(_isLoaded);
            }
        }

        public AdvertisementReward(IAdvertisementWrapper advertisementWrapper)
        {
            _advertisementWrapper = advertisementWrapper;
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"AdvertisementReward - OnUnityAdsLoaded({placementId}) ");
            _taskLoadCompletionSource.SetResult(true);
            IsLoaded = true;
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"AdvertisementReward - OnUnityAdsFailedToLoad({placementId}) " + message);
            _taskLoadCompletionSource.SetResult(false);
            IsLoaded = false;
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
            bool isLoaded = _advertisementWrapper.IsReady(placementId);
            if (!isLoaded)
                _advertisementWrapper.Load(placementId, this);
            else
            {
                _taskLoadCompletionSource.SetResult(true);
                Debug.Log($"AdvertisementReward - {placementId} already Ready");
            }

            Debug.Log($"AdvertisementReward - Load({placementId}) setting _placementId");
            _placementId = placementId;

            return _taskLoadCompletionSource.Task;
        }

        private void Reload()
        {
            if (_placementId == string.Empty || IsLoaded) return;
            _taskLoadCompletionSource = new TaskCompletionSource<bool>();
            _advertisementWrapper.Load(_placementId, this);
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
            IsLoaded = false;
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
                new RewardAdvertisementFinishedArgs((RewardAdvertisementResult)showCompletionState)
            );
            IsLoaded = false;
            Reload();
        }
    }
}