using System.Threading.Tasks;

namespace Jturesson.Advertisements
{
    public interface IAdvertisementReward
    {
        Task<bool> Load(string placementId);
    }
}