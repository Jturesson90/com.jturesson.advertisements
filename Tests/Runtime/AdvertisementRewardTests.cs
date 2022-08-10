using System.Threading.Tasks;
using JTuresson.Advertisements;
using NSubstitute;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.Advertisements;

namespace Tests.Runtime
{
    public class AdvertisementRewardTests
    {
        [Test]
        [TestCase(RewardAdvertisementResult.Completed, "123")]
        [TestCase(RewardAdvertisementResult.Skipped, "321")]
        [TestCase(RewardAdvertisementResult.Unknown, "213")]
        public void RewardAdvertisementFinishedArgsHasCorrectMembers(RewardAdvertisementResult e,
            string referenceId)
        {
            var rewardAdArgs = new RewardAdvertisementFinishedArgs(e);

            Assert.That(rewardAdArgs.ShowResult, Is.EqualTo(e));
        }

        [UnityTest]
        public IEnumerator RewardAdvertisementShowCompleted()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            string placementId = "placementIdTest";
            // Need to load to set the placementId;
            wrapper.WhenForAnyArgs(x =>
                    x.Load(placementId, Arg.Any<IUnityAdsLoadListener>()))
                .Do(y => y.Arg<IUnityAdsLoadListener>()
                    .OnUnityAdsAdLoaded(placementId));
            wrapper.WhenForAnyArgs(x =>
                    x.Show(placementId, Arg.Any<IUnityAdsShowListener>()))
                .Do(y => y.Arg<IUnityAdsShowListener>()
                    .OnUnityAdsShowComplete(placementId, UnityAdsShowCompletionState.COMPLETED));

            IAdvertisementReward advertisementReward =
                new AdvertisementReward(wrapper);


            var i = advertisementReward.Load(placementId);
            yield return i.Result;
            var j = advertisementReward.Show();
            yield return j.Result;


            Assert.IsTrue(j.Result.ShowResult == RewardAdvertisementResult.Completed);
        }

        [UnityTest]
        public IEnumerator RewardAdvertisementShowSkipped()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            IAdvertisementReward advertisementReward =
                new AdvertisementReward(wrapper);
            string placementId = "placementIdTest";

            // Need to load to set the placementId;
            wrapper.WhenForAnyArgs(x =>
                    x.Load(Arg.Any<string>(), Arg.Any<IUnityAdsLoadListener>()))
                .Do(y => y.Arg<IUnityAdsLoadListener>()
                    .OnUnityAdsAdLoaded(placementId));
            var i = Task.Run(() => advertisementReward.Load(placementId));
            yield return i.Result;

            wrapper.WhenForAnyArgs(x =>
                    x.Show(Arg.Any<string>(), Arg.Any<IUnityAdsShowListener>()))
                .Do(y => y.Arg<IUnityAdsShowListener>()
                    .OnUnityAdsShowComplete(placementId, UnityAdsShowCompletionState.SKIPPED));
            var j = Task.Run(() => advertisementReward.Show());
            yield return j.Result;

            Assert.IsTrue(j.Result.ShowResult == RewardAdvertisementResult.Skipped);
        }
    }
}