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
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.Runtime
{
    public class AdvertisementManagerBehaviourTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [UnityTest]
        public IEnumerator CanInitialize()
        {
            /*
                        var advertisementInitializer =
                            Substitute.For<IAdvertisementInitializer>();
            
                        var unityAdsInitialization = Substitute.For<IUnityAdsInitialization>();
                        unityAdsInitialization.Initialize().Returns(true);
                        var advertisementWrapper = Substitute.For<IAdvertisementWrapper>();
                        var applicationWrapper = Substitute.For<IApplicationWrapper>();
                        applicationWrapper.Platform.Returns(RuntimePlatform.Android);
                        advertisementInitializer.AdvertisementPlatformSettings.Returns(
                            new AdvertisementPlatformSettings[]
                            {
                                new()
                                {
                                    runtimePlatform = RuntimePlatform.Android
                                }
                            });
                        _advertisement.Inject(advertisementInitializer, unityAdsInitialization,
                            advertisementWrapper, applicationWrapper);
            
                        var t = _advertisement.Initialize();
            
                        yield return t.Result;
                        Assert.IsTrue(t.Result);*/
            yield return null;
        }

        [UnityTest]
        public IEnumerator CurrentRuntimeDontExistInListThrowsError()
        {
            var g = new GameObject("AdvertisementManagerBehaviour");
            var advertisementManager = g.AddComponent<AdvertisementManagerBehaviour>();
            var advertisementInitializer =
                Substitute.For<IAdvertisementInitializer>();
            var unityAdsInitialization = Substitute.For<IUnityAdsInitialization>();
            unityAdsInitialization.Initialize().Returns(Task.FromResult(true));

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
                advertisementWrapper, applicationWrapper);
            var s = Task.Run(() => advertisementManager.Initialize());
            while (!s.IsCompleted)
            {
                yield return null;
            }

            Assert.IsTrue(s.Exception is {InnerException: NotSupportedException});

            Object.DestroyImmediate(g);
        }
    }
}