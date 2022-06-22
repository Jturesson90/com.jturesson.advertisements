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
        public AdPlacementBanner banner;
        public AdPlacement interstitial;
        public AdPlacement reward;
    }
}