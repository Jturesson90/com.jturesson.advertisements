using System.Threading.Tasks;
using UnityEngine.Advertisements;

namespace Jturesson.Advertisements
{
    public class UnityAdsInitialization : IUnityAdsInitialization, IUnityAdsInitializationListener
    {
        private readonly string _gameId;
        private readonly bool _testMode;
        private readonly bool _enablePerPlacementLoad;
        private readonly IAdvertisementWrapper _advertisementWrapper;

        public UnityAdsInitialization(IAdvertisementWrapper advertisementWrapper, string gameId,
            bool testMode = false, bool enablePerPlacementLoad = false)
        {
            _advertisementWrapper = advertisementWrapper;
            _gameId = gameId;
            _testMode = testMode;
            _enablePerPlacementLoad = enablePerPlacementLoad;
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

        public async Task<bool> Initialize()
        {
            _initializationTcs = new TaskCompletionSource<bool>();

            _advertisementWrapper.Initialize(_gameId, _testMode, _enablePerPlacementLoad, this);

            return await _initializationTcs.Task;
        }
    }
}