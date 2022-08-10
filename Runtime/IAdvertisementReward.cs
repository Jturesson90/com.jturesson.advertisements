using System;
using System.Threading.Tasks;

namespace JTuresson.Advertisements
{
    public interface IAdvertisementReward
    {
        Task<bool> Load(string placementId);
        Task<RewardAdvertisementFinishedArgs> Show();
        event Action<bool> IsLoadedChanged;
    }
}