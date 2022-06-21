using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Jturesson.Advertisements;
using UnityEngine;
using NUnit.Framework;
using NSubstitute;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.Runtime
{
    
    public class AdvertisementManagerBehaviourTests
    {
        private AdvertisementManagerBehaviour _advertisement;

        [SetUp]
        public void SetUp()
        {
            var g = new GameObject("AdvertisementManagerBehaviour");
            _advertisement = g.AddComponent<AdvertisementManagerBehaviour>();
        }

        [UnityTest]
        public IEnumerator CanInitialize()
        {
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
            Assert.IsTrue(t.Result);
        }


        public IEnumerator CurrentRuntimeDontExistInListThrowsError()
        {
            var advertisementInitializer =
                Substitute.For<IAdvertisementInitializer>();

            var unityAdsInitialization = Substitute.For<IUnityAdsInitialization>();
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

            var t = Task.Run(() =>
                TaskExtensions.AssertThrowsAsync<NotSupportedException>(
                    _advertisement.Initialize
                ));
            yield return t.AsIEnumeratorReturnNull();

            Assert.Fail();
        }


        public IEnumerator CurrentRuntimeDontExistInListThrowsError2()
        {
            var advertisementInitializer =
                Substitute.For<IAdvertisementInitializer>();

            var unityAdsInitialization = Substitute.For<IUnityAdsInitialization>();
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

            _advertisement.Inject(advertisementInitializer, unityAdsInitialization,
                advertisementWrapper, applicationWrapper);

            Assert.Throws<NotSupportedException>(() =>
            {
                try
                {
                    _advertisement.Initialize();
                }
                catch (Exception e)
                {
                    throw e;
                }
            });

            yield return null;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_advertisement.gameObject);
        }
    }

    public static class TaskExtensions
    {
        public static IEnumerator AsIEnumeratorReturnNull<T>(this Task<T> task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                if (task.Exception != null) ExceptionDispatchInfo.Capture(task.Exception).Throw();
            }

            yield return null;
        }

        public static TActual AssertThrowsAsync<TActual>(AsyncTestDelegate code, string message = "",
            params object[] args) where TActual : Exception
        {
            return Assert.Throws<TActual>(() =>
            {
                try
                {
                    code.Invoke().Wait(); // Will wrap any exceptions in an AggregateException
                }
                catch (AggregateException e)
                {
                    if (e.InnerException is null)
                    {
                        throw;
                    }

                    throw e.InnerException; // Throw the unwrapped exception
                }
            }, message, args);
        }

        public delegate Task AsyncTestDelegate();
    }
}