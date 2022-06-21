using System.Collections;
using System.Collections.Generic;
using Jturesson.Advertisements;
using NSubstitute;
using NUnit.Framework;
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
                new UnityAdsInitialization(wrapper, gameId, testMode, enablePerPlacementLoad);
            adsInit.Initialize();
            wrapper.Received().Initialize(gameId, testMode, enablePerPlacementLoad, adsInit);
        }

        [UnityTest]
        public IEnumerator InitializeCanFail()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            var adsInit = new UnityAdsInitialization(wrapper, "gameId", true, true);
            wrapper.WhenForAnyArgs(x =>
                    x.Initialize(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(),
                        Arg.Any<IUnityAdsInitializationListener>()))
                .Do(y => adsInit.OnInitializationFailed(UnityAdsInitializationError.AD_BLOCKER_DETECTED,
                    ""));

            var a = adsInit.Initialize();

            yield return a.Result;
            Assert.That(a.Result, Is.False);
        }

        [UnityTest]
        public IEnumerator InitializeCanInitialize()
        {
            var wrapper = Substitute.For<IAdvertisementWrapper>();
            var adsInit = new UnityAdsInitialization(wrapper, "gameId", true, true);
            wrapper.WhenForAnyArgs(x =>
                    x.Initialize(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>(),
                        Arg.Any<IUnityAdsInitializationListener>()))
                .Do(y => adsInit.OnInitializationComplete());
            var a = adsInit.Initialize();

            yield return a.Result;
            Assert.That(a.Result, Is.True);
        }
    }
}