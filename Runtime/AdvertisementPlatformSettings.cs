using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Jturesson.Advertisements
{
    [Serializable]
    public class AdvertisementPlatformSettings
    {
        public RuntimePlatform runtimePlatform;
        public string gameId;
        public BannerAdPlacement banner;
        public AdPlacement interstitial;
        public AdPlacement reward;
    }
}