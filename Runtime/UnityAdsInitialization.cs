using System.Threading.Tasks;
using UnityEngine.Advertisements;

namespace JTuresson.Advertisements
{
    public class UnityAdsInitialization : IUnityAdsInitialization, IUnityAdsInitializationListener
    {
        private readonly IAdvertisementWrapper _advertisementWrapper;

        public UnityAdsInitialization(IAdvertisementWrapper advertisementWrapper)
        {
            _advertisementWrapper = advertisementWrapper;
        }

        public void OnInitializationComplete()
        {
            _initializationTcs.SetResult(true);
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            _initializationTcs.SetResult(false);
        }

        private TaskCompletionSource<bool> _initializationTcs;

        public async Task<bool> Initialize(string gameId, bool testMode = false,
            bool enablePerPlacementLoad = false)
        {
            _initializationTcs = new TaskCompletionSource<bool>();
            _advertisementWrapper.Initialize(gameId, testMode, enablePerPlacementLoad, this);

            return await _initializationTcs.Task;
        }
    }
}