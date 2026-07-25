using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseGun : MonoBehaviour
{
    [Header("Base gun settings")]
    [SerializeField] protected int bulletCostPerShot = 1;
    [SerializeField] protected float fireRate = 0.5f;
    [SerializeField] protected LayerMask hitLayers;
    
    [Header("2D UI References")]
    [SerializeField] protected RawImage gunImage;
    [SerializeField] protected Animator gunAnimator;
    
    [Header("3D Shooting References")]
    [SerializeField] protected Camera playerCamera;

    protected float _nextFireTime;

    protected bool TryHitScan(Ray ray, float range, out RaycastHit hit)
    {
        var layers = hitLayers.value == 0 ? ~0 : hitLayers.value;
        return Physics.Raycast(ray, out hit, range, layers, QueryTriggerInteraction.Ignore);
    }

    protected bool TryGetDamageable(Collider collider, out IDamageable damageable)
    {
        var behaviours = collider.GetComponentsInParent<MonoBehaviour>();

        for (var i = 0; i < behaviours.Length; i++)
        {
            if (behaviours[i] is IDamageable found)
            {
                damageable = found;
                return true;
            }
        }

        damageable = null;
        return false;
    }

    protected void DealInfinitePiercingDamage(Ray ray, float range, float damage)
    {
        var layers = hitLayers.value == 0 ? ~0 : hitLayers.value;
        var hits = Physics.RaycastAll(ray, range, layers, QueryTriggerInteraction.Ignore);
        Array.Sort(hits, (left, right) => left.distance.CompareTo(right.distance));

        var damagedTargets = new HashSet<IDamageable>();

        for (var i = 0; i < hits.Length; i++)
        {
            if (!TryGetDamageable(hits[i].collider, out var target))
            {
                continue;
            }

            if (damagedTargets.Add(target))
            {
                target.TakeDamage(PlayerData.Instance.isDoubleDmgMul ? damage * 2f : damage);
            }
        }
    }

    public abstract void TryShoot();
    
    protected abstract void FirePattern();

    protected void PlayAnimationOnce(string stateName)
    {
        StartCoroutine(PlayAndReturnRoutine(stateName));
    }

    protected abstract IEnumerator PlayAndReturnRoutine(string stateName);
}
