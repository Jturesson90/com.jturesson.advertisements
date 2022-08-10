using System;
using UnityEditor;
using UnityEngine.Advertisements;

namespace JTuresson.Advertisements
{
    public interface IAdvertisementInitializer
    {
        bool TestMode { get; }
        bool EnablePerPlacementLoad { get; }
        AdvertisementPlatformSettings[] AdvertisementPlatformSettings { get; }
    }
}