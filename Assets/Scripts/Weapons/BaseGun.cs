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
