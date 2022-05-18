using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        var a = Advertisement.isShowing;
    }
}