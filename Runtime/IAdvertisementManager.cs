using System;
using System.Threading.Tasks;

namespace JTuresson.Advertisements
{
    public interface IAdvertisementManager
    {
        Task<bool> Initialize();
        void ShowBanner();
        void HideBanner();
        bool BannerIsReadyAndEnabled();
        bool InterstitialIsReadyAndEnabled();
        void ShowInterstitial();
        bool RewardIsReadyAndEnabled();
        Task<RewardAdvertisementFinishedArgs> ShowReward();
        event Action<bool> RewardIsReadyChanged;
    }
}