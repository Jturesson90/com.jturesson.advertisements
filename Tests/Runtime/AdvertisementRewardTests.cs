using System.Collections.Generic;
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
            var rewardAdArgs = new RewardAdvertisementFinishedArgs(e, referenceId);

            Assert.That(rewardAdArgs.ReferenceId, Is.EqualTo(referenceId));
            Assert.That(rewardAdArgs.ShowResult, Is.EqualTo(e));
        }

        [UnityTest]
        public IEnumerator RewardAdvertisementShowCompleted()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            IAdvertisementReward advertisementReward =
                new JTuresson.Advertisements.AdvertisementReward(wrapper);
            string placementId = "placementIdTest";

            // Need to load to set the placementId;
            wrapper.WhenForAnyArgs(x =>
                    x.Load(Arg.Any<string>(), Arg.Any<IUnityAdsLoadListener>()))
                .Do(y => y.Arg<IUnityAdsLoadListener>()
                    .OnUnityAdsAdLoaded(placementId));
            var i = Task.Run(() => advertisementReward.Load(placementId));
            while (!i.IsCompleted)
            {
                yield return null;
            }

            wrapper.WhenForAnyArgs(x =>
                    x.Show(Arg.Any<string>(), Arg.Any<IUnityAdsShowListener>()))
                .Do(y => y.Arg<IUnityAdsShowListener>()
                    .OnUnityAdsShowComplete("", UnityAdsShowCompletionState.COMPLETED));
            var j = Task.Run(() => advertisementReward.Show());
            while (!j.IsCompleted)
            {
                yield return null;
            }

            Assert.IsTrue(j.Result.ShowResult == RewardAdvertisementResult.Completed);
        }

        [UnityTest]
        public IEnumerator RewardAdvertisementShowSkipped()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            IAdvertisementReward advertisementReward =
                new JTuresson.Advertisements.AdvertisementReward(wrapper);
            string placementId = "placementIdTest";

            // Need to load to set the placementId;
            wrapper.WhenForAnyArgs(x =>
                    x.Load(Arg.Any<string>(), Arg.Any<IUnityAdsLoadListener>()))
                .Do(y => y.Arg<IUnityAdsLoadListener>()
                    .OnUnityAdsAdLoaded(placementId));
            var i = Task.Run(() => advertisementReward.Load(placementId));
            while (!i.IsCompleted)
            {
                yield return null;
            }

            wrapper.WhenForAnyArgs(x =>
                    x.Show(Arg.Any<string>(), Arg.Any<IUnityAdsShowListener>()))
                .Do(y => y.Arg<IUnityAdsShowListener>()
                    .OnUnityAdsShowComplete("", UnityAdsShowCompletionState.SKIPPED));
            var j = Task.Run(() => advertisementReward.Show());
            while (!j.IsCompleted)
            {
                yield return null;
            }

            Assert.IsTrue(j.Result.ShowResult == RewardAdvertisementResult.Skipped);
        }
    }
}