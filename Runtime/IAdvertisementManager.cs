using System;
using System.Threading.Tasks;

namespace Jturesson.Advertisements
{
    public interface IAdvertisementManager
    {
        Task<bool> Initialize();
        void ShowBanner();
        void HideBanner();
    }
}