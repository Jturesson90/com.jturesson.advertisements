using System.Threading.Tasks;

namespace JTuresson.Advertisements
{
    public interface IAdvertisementInterstitial
    {
        Task<bool> Load(string placementId);
        void Show();
        bool IsLoaded { get; }
    }
}