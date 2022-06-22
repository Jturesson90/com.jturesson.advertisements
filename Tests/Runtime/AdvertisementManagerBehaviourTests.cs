using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Jturesson.Advertisements;
using UnityEngine;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.Core;
using UnityEngine.Advertisements;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.Runtime
{
    public class AdvertisementManagerBehaviourTests
    {
        [UnityTest]
        public IEnumerator CanInitialize()
        {
            var g = new GameObject(
                "AdvertisementManagerBehaviour CanInitialize");
            var advertisementManager = g.AddComponent<AdvertisementManagerBehaviour>();
            var advertisementInitializer =
                Substitute.For<IAdvertisementInitializer>();
            var unityAdsInitialization = Substitute.For<IUnityAdsInitialization>();
            unityAdsInitialization.Initialize(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(Task.FromResult(true));

            var advertisementWrapper = Substitute.For<IAdvertisementWrapper>();
            var applicationWrapper = Substitute.For<IApplicationWrapper>();
            applicationWrapper.Platform.Returns(RuntimePlatform.Android);
            advertisementInitializer.AdvertisementPlatformSettings.Returns(
                new AdvertisementPlatformSettings[]
                {
                    new()
                    {
                        runtimePlatform = RuntimePlatform.Android,
                        banner = new AdPlacementBanner()
                        {
                            enabled = true, bannerPosition = BannerPosition.CENTER,
                            placementId = "banner"
                        },
                        interstitial = new AdPlacement() {enabled = true, placementId = "interstitial"},
                        reward = new AdPlacement() {enabled = true, placementId = "reward"}
                    }
                });
            var advertisementInterstitial = Substitute.For<IAdvertisementInterstitial>();
            var advertisementBanner = Substitute.For<IAdvertisementBanner>();
            var advertisementReward = Substitute.For<IAdvertisementReward>();

            advertisementInterstitial.Load(Arg.Any<string>()).Returns(Task.FromResult(true));
            advertisementBanner.Load(Arg.Any<string>(), Arg.Any<BannerPosition>())
                .Returns(Task.FromResult(true));
            advertisementReward.Load(Arg.Any<string>()).Returns(Task.FromResult(true));

            advertisementManager.Inject(advertisementInitializer, unityAdsInitialization,
                advertisementWrapper, applicationWrapper, advertisementInterstitial,
                advertisementBanner, advertisementReward);


            var s = Task.Run(() => advertisementManager.Initialize());
            while (!s.IsCompleted)
            {
                yield return null;
            }

            Assert.IsTrue(s.Result);

            Object.DestroyImmediate(g);
        }

        [UnityTest]
        public IEnumerator CurrentRuntimePlatformWhenNotExistInListThrowsError()
        {
            var g = new GameObject(
                "AdvertisementManagerBehaviour CurrentRuntimePlatformWhenNotExistInListThrowsError");
            var advertisementManager = g.AddComponent<AdvertisementManagerBehaviour>();
            var advertisementInitializer =
                Substitute.For<IAdvertisementInitializer>();
            var unityAdsInitialization = Substitute.For<IUnityAdsInitialization>();
            unityAdsInitialization.Initialize(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(Task.FromResult(true));

            var advertisementWrapper = Substitute.For<IAdvertisementWrapper>();
            var applicationWrapper = Substitute.For<IApplicationWrapper>();
            applicationWrapper.Platform.Returns(RuntimePlatform.Lumin);
            advertisementInitializer.AdvertisementPlatformSettings.Returns(
                new AdvertisementPlatformSettings[]
                {
                    new()
                    {
                        runtimePlatform = RuntimePlatform.Android
                    }
                });

            advertisementManager.Inject(advertisementInitializer, unityAdsInitialization,
                advertisementWrapper, applicationWrapper, null,
                null, null);
            var s = Task.Run(() => advertisementManager.Initialize());
            while (!s.IsCompleted)
            {
                yield return null;
            }

            // Assert
            Assert.IsTrue(s.Exception is {InnerException: NotSupportedException});

            // Clean up
            Object.DestroyImmediate(g);
        }
    }
}