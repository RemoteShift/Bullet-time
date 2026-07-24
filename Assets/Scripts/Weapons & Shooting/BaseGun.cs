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
    // [SerializeField] protected Animator gunAnimator;
    
    [Header("3D Shooting References")]
    [SerializeField] protected Camera playerCamera;

    private float _nextFireTime;

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

    public void TryShoot()
    {
        if(Time.time < _nextFireTime)
        {
            return;
        }

        if(!BulletManager.Instance.TryTakeBullets(bulletCostPerShot))
        {
            return;
        }
        
        _nextFireTime = Time.time + fireRate;

        FirePattern();
    }
    
    protected abstract void FirePattern();
}
