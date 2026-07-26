using System;
using UnityEngine;

public class ShopTransitionHandler : Singleton<ShopTransitionHandler>
{
    [HideInInspector] public string sceneName;
    [HideInInspector] public int nextBulletCap;
    [HideInInspector] public Action onComplete;
    
    public void Initialize(string scene, int bulletCap, Action action)
    {
        sceneName = scene;
        nextBulletCap = bulletCap;
        onComplete = action;
    }
}
