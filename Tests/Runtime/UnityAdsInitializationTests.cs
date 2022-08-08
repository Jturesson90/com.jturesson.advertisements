using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jturesson.Advertisements;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    [Category("Initialization")]
    public class UnityAdsInitializationTests
    {
        [Test]
        [TestCase("", true, true)]
        [TestCase("gameId", false, false)]
        public void InitializeReceivedWithParameters(string gameId, bool testMode,
            bool enablePerPlacementLoad)
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();

            var adsInit =
                new UnityAdsInitialization(wrapper);
            adsInit.Initialize(gameId, testMode, enablePerPlacementLoad);
            wrapper.Received().Initialize(gameId, testMode, enablePerPlacementLoad, adsInit);
        }

        [UnityTest]
        public IEnumerator InitializeCanFail()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            var adsInit = new UnityAdsInitialization(wrapper);
            wrapper.WhenForAnyArgs(x =>
                    x.Initialize(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(),
                        Arg.Any<IUnityAdsInitializationListener>()))
                .Do(y => y.Arg<IUnityAdsInitializationListener>().OnInitializationFailed(
                    UnityAdsInitializationError.AD_BLOCKER_DETECTED,
                    ""));

            var a = adsInit.Initialize("gameId", true, true);

            yield return a.Result;
            Assert.That(a.Result, Is.False);
        }

        [UnityTest]
        public IEnumerator InitializeCanInitialize()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            var adsInit = new UnityAdsInitialization(wrapper);
            wrapper.WhenForAnyArgs(x =>
                    x.Initialize(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(),
                        Arg.Any<IUnityAdsInitializationListener>()))
                .Do(y => adsInit.OnInitializationComplete());
            var a = adsInit.Initialize("gameId", true, true);

            yield return a.Result;
            Assert.That(a.Result, Is.True);
        }

        [UnityTest]
        public IEnumerator BannerCanLoad()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            var banner = new AdvertisementBanner(wrapper);

            wrapper.WhenForAnyArgs(x =>
                    x.BannerLoad(Arg.Any<string>(), Arg.Any<BannerLoadOptions>()))
                .Do(y => y.Arg<BannerLoadOptions>().loadCallback());

            var a = banner.Load("gameId", BannerPosition.TOP_CENTER);

            yield return a.Result;
            Assert.That(a.Result, Is.True);
        }

        [UnityTest]
        public IEnumerator BannerCanFailLoad()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            var banner = new AdvertisementBanner(wrapper);

            wrapper.WhenForAnyArgs(x =>
                    x.BannerLoad(Arg.Any<string>(), Arg.Any<BannerLoadOptions>()))
                .Do(y => y.Arg<BannerLoadOptions>().errorCallback(""));

            var a = banner.Load("gameId", BannerPosition.TOP_CENTER);

            yield return a.Result;
            Assert.That(a.Result, Is.False);
        }

        [UnityTest]
        public IEnumerator InterstitialCanLoad()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            var interstitial = new AdvertisementInterstitial(wrapper);

            wrapper.WhenForAnyArgs(x =>
                    x.Load(Arg.Any<string>(), Arg.Any<IUnityAdsLoadListener>()))
                .Do(y => y.Arg<IUnityAdsLoadListener>().OnUnityAdsAdLoaded("gameId"));

            var a = interstitial.Load("gameId");

            yield return a.Result;
            Assert.That(a.Result, Is.True);
        }

        [UnityTest]
        public IEnumerator InterstitialCanLoadFail()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            var interstitial = new AdvertisementInterstitial(wrapper);

            wrapper.WhenForAnyArgs(x =>
                    x.Load(Arg.Any<string>(), Arg.Any<IUnityAdsLoadListener>()))
                .Do(y => y.Arg<IUnityAdsLoadListener>()
                    .OnUnityAdsFailedToLoad("gameId", UnityAdsLoadError.NO_FILL, ""));

            var a = interstitial.Load("gameId");

            yield return a.Result;
            Assert.That(a.Result, Is.False);
        }

        [UnityTest]
        public IEnumerator RewardCanLoad()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            var reward = new AdvertisementReward(wrapper);

            wrapper.WhenForAnyArgs(x =>
                    x.Load(Arg.Any<string>(), Arg.Any<IUnityAdsLoadListener>()))
                .Do(y => y.Arg<IUnityAdsLoadListener>().OnUnityAdsAdLoaded("gameId"));

            var a = reward.Load("gameId");

            yield return a.Result;
            Assert.That(a.Result, Is.True);
        }

        [UnityTest]
        public IEnumerator RewardCanLoadFail()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            var reward = new AdvertisementReward(wrapper);

            wrapper.WhenForAnyArgs(x =>
                    x.Load(Arg.Any<string>(), Arg.Any<IUnityAdsLoadListener>()))
                .Do(y => y.Arg<IUnityAdsLoadListener>()
                    .OnUnityAdsFailedToLoad("gameId", UnityAdsLoadError.NO_FILL, ""));

            var a = reward.Load("gameId");

            yield return a.Result;
            Assert.That(a.Result, Is.False);
        }

        [UnityTest]
        public IEnumerator CanNotStartLoadWhileLoading()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            var reward = new AdvertisementReward(wrapper);
            var interstitial = new AdvertisementInterstitial(wrapper);
            wrapper.WhenForAnyArgs(x =>
                    x.Load(Arg.Any<string>(), Arg.Any<IUnityAdsLoadListener>()))
                .Do(y =>
                {
                    Task.Run(delegate
                    {
                        y.Arg<IUnityAdsLoadListener>()
                            .OnUnityAdsAdLoaded("gameId");
                    });
                });

            var a = reward.Load("gameId");
            var b = reward.Load("gameId");
            var aa = interstitial.Load("");
            var bb = interstitial.Load("");
            yield return a.Result;
            yield return b.Result;
            yield return aa.Result;
            yield return bb.Result;
            Assert.That(a.Result, Is.True);
            Assert.That(aa.Result, Is.True);
            Assert.That(b.Result, Is.False);
            Assert.That(bb.Result, Is.False);
        }

        [UnityTest]
        public IEnumerator BannerCanNotStartLoadWhileLoading()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            var banner = new AdvertisementBanner(wrapper);
            wrapper.WhenForAnyArgs(x =>
                    x.BannerLoad(Arg.Any<string>(), Arg.Any<BannerLoadOptions>()))
                .Do(y =>
                {
                    Task.Run(delegate
                    {
                        y.Arg<BannerLoadOptions>()
                            .loadCallback();
                    });
                });

            var a = banner.Load("gameId");
            var b = banner.Load("gameId");
            yield return a.Result;
            yield return b.Result;
            Assert.That(a.Result, Is.True);
            Assert.That(b.Result, Is.False);
        }
    }
}