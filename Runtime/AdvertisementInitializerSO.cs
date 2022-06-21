using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Jturesson.Advertisements
{
    [CreateAssetMenu(menuName = "JTuresson/Advertisements", fileName = "New AdvertisementInitializer")]
    public class AdvertisementInitializerSO : ScriptableObject, IAdvertisementInitializer
    {
        [field: SerializeField, Header("Initialization")]
        public bool TestMode { get; set; }

        [field: SerializeField] public bool EnablePerPlacementLoad { get; set; }

        [field: SerializeField]
        public AdvertisementPlatformSettings[] AdvertisementPlatformSettings { get; set; }
    }
}