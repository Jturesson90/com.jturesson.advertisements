using Jturesson.Advertisements;
using NUnit.Framework;

namespace Tests.Runtime
{
    public class RewardAdvertisementFinishedArgsTests
    {
        [Test]
        [TestCase(RewardAdvertisementResult.Completed, "123")]
        [TestCase(RewardAdvertisementResult.Skipped, "321")]
        [TestCase(RewardAdvertisementResult.Unknown, "213")]
        public void RewardAdvertisementFinishedArgsHasCorrectMembers(RewardAdvertisementResult e,
            string referenceId)
        {
            var rewardAdArgs = new RewardAdvertisementFinishedArgs(e, referenceId);

            Assert.That(rewardAdArgs.ReferenceId, Is.EqualTo(referenceId));
            Assert.That(rewardAdArgs.ShowResult, Is.EqualTo(e));
        }
    }
}