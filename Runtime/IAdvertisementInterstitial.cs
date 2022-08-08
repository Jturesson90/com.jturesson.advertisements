using System.Threading.Tasks;

namespace Jturesson.Advertisements
{
    public interface IAdvertisementInterstitial
    {
        Task<bool> Load(string placementId);
        void Show();
    }
}