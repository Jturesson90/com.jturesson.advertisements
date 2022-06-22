using System.Threading.Tasks;
using UnityEngine.Advertisements;

namespace Jturesson.Advertisements
{
    public interface IAdvertisementBanner
    {
        Task<bool> Load(string placementId,
            BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER);

        void Show();
        void Hide();
        bool IsLoaded();
    }
}