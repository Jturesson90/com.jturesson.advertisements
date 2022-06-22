using System.Threading.Tasks;
using UnityEngine.Advertisements;

namespace Jturesson.Advertisements
{
    public class AdvertisementBanner : IAdvertisementBanner
    {
        private readonly IAdvertisementWrapper _advertisementWrapper;

        private TaskCompletionSource<bool> _taskCompletionSource;

        private string _placementId;

        public AdvertisementBanner(IAdvertisementWrapper advertisementWrapper)
        {
            _advertisementWrapper = advertisementWrapper;
        }

        public Task<bool> Load(string placementId,
            BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER)
        {
            if (_taskCompletionSource != null && _taskCompletionSource.Task.Status == TaskStatus.Running)
                return Task.FromResult(false);

            _taskCompletionSource = new TaskCompletionSource<bool>();
            _advertisementWrapper.BannerSetPosition(bannerPosition);

            var options = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };
            _advertisementWrapper.BannerLoad(placementId, options);
            _placementId = placementId;

            return _taskCompletionSource.Task;
        }

        public void Show()
        {
            if (!_advertisementWrapper.BannerIsLoaded) return;
            _advertisementWrapper.BannerShow(_placementId, new BannerOptions()
            {
                clickCallback = ClickCallback,
                hideCallback = HideCallback,
                showCallback = ShowCallback
            });
        }

        private void ShowCallback()
        {
        }

        private void HideCallback()
        {
        }

        private void ClickCallback()
        {
        }

        public void Hide()
        {
            _advertisementWrapper.BannerHide();
        }

        public bool IsLoaded()
        {
            return _advertisementWrapper.BannerIsLoaded;
        }

        private void OnBannerError(string message)
        {
            _taskCompletionSource.SetResult(false);
        }

        private void OnBannerLoaded()
        {
            _taskCompletionSource.SetResult(true);
        }
    }
}