using System.Collections;
using UnityEngine;

public class Pistol : BaseGun
{
    [Header("Pistol Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;

    public override void TryShoot()
    {
        if(Time.time < _nextFireTime)
        {
            return;
        }

        if(!BulletManager.Instance.TryTakeBullets(bulletCostPerShot))
        {
            return;
        }
        
        _nextFireTime = Time.time + (PlayerData.Instance.isDoublePistolFireRate ? 0.5f : 1) * fireRate;

        FirePattern();
        PlayAnimationOnce("Shooting");
        AudioManager.Instance.PlayGlobalSFX(PlayerDmgDealer.Instance.pistolShoot, 0.8f);
    }

    protected override void FirePattern()
    {
        var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if(PlayerData.Instance.isPistolPiercing)
        {
            DealInfinitePiercingDamage(ray, range, damage);
        }
        else
        {
            if (TryHitScan(ray, range, out var hit) && TryGetDamageable(hit.collider, out var target))
            {
                target.TakeDamage(PlayerData.Instance.isDoubleDmgMul ? damage * 2f : damage);
            }
        }
    }

    protected override IEnumerator PlayAndReturnRoutine(string stateName)
    {
        var speed = PlayerData.Instance.isDoublePistolFireRate ? 2f : 1f;
        
        gunAnimator.Play(stateName, 0, 0f);
        gunAnimator.SetFloat("ShootingSpeedMul", speed);
        
        yield return null;
        
        var info = gunAnimator.GetCurrentAnimatorStateInfo(0);
        var duration = info.length / speed;
        
        yield return new WaitForSeconds(duration);
        
        gunAnimator.Play("Idle"); 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * range);
    }
}
