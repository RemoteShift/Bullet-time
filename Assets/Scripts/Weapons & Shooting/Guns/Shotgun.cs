using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shotgun : BaseGun
{
    [Header("Shotgun Settings")]
    [SerializeField] private int pelletCount = 8;
    [SerializeField] private float damagePerPellet = 10f;
    [SerializeField] private float range = 15f;
    [SerializeField] private float spreadAngle = 6f;

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
        
        _nextFireTime = Time.time + (PlayerData.Instance.isDoubleShotgunFireRate ? 0.5f : 1) * fireRate;

        FirePattern();
        PlayAnimationOnce("Shooting");
    }

    protected override void FirePattern()
    {
        var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        for (var i = 0; i < pelletCount; i++)
        {
            var targetDirection = playerCamera.transform.forward;
            var randomSpread = Random.insideUnitCircle * Mathf.Tan(spreadAngle * Mathf.Deg2Rad);
            
            targetDirection += playerCamera.transform.right * randomSpread.x;
            targetDirection += playerCamera.transform.up * randomSpread.y;
            targetDirection.Normalize();

            var spreadRay = new Ray(ray.origin, targetDirection);

            if (TryHitScan(spreadRay, range, out var hit))
            {
                if (TryGetDamageable(hit.collider, out var target))
                {
                    target.TakeDamage(PlayerData.Instance.isDoubleDmgMul ? damagePerPellet * 2f : damagePerPellet);
                }
            }
        }
    }

    protected override IEnumerator PlayAndReturnRoutine(string stateName)
    {
        var speed = PlayerData.Instance.isDoubleShotgunFireRate ? 2f : 1f;
        
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
