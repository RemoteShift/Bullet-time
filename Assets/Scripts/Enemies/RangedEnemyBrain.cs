using System;
using UnityEngine;

public class RangedEnemyBrain : EnemyBrain
{
    [Header("Ranged Attack Settings")]
    public float preferredShootingDistance = 12f;
    public float nextAttackTime;
    
    [Header("Spawn Settings")]
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private Transform eyePoint;

    [Header("Line of Sight Layers")]
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private LayerMask playerLayer;

    [Header("Other Settings")]
    [SerializeField] private GameObject _bulletPickupPrefab;

    [Header("SFX")] 
    public AudioClip rangedAttack;
    
    public override void ReturnToDefaultState()
    {
        SwitchState(new EnemyIdleState(this, new RangedChaseState(this)));
    }
    
    public override bool CanSeeTarget()
    {
        return base.CanSeeTarget() && HasLineOfSight();
    }
    
    public bool HasLineOfSight()
    {
        if (!target) return false;

        var origin = eyePoint ? eyePoint.position : transform.position + Vector3.up * 1.5f;
        var targetPos = target.position;
        var direction = (targetPos - origin).normalized;
        var distance = Vector3.Distance(origin, targetPos);

        LayerMask combinedLayers = obstacleLayers | playerLayer;

        if (Physics.Raycast(origin, direction, out var hit, distance, combinedLayers))
        {
            // If the first collider we hit belongs to the player, line of sight is clear
            if (hit.collider.transform == target || ((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
            {
                return true;
            }
        }

        return false;
    }
    
    public void FireProjectile()
    {
        if (!target) return;
        
        enemyAudio.PlayEnemySound(rangedAttack, volumeMult: 2f);

        var spawnPos = muzzlePoint ? muzzlePoint.position : transform.position + Vector3.up * 1.5f;
        
        // Aim toward player target center (with slight vertical offset for chest level)
        var targetCenter = target.position + Vector3.up * 0.5f;
        var aimDirection = (targetCenter - spawnPos).normalized;
        var fireRotation = Quaternion.LookRotation(aimDirection);

        // Fetch pooled bullet instead of Instantiating
        EnemyBulletPool.Instance.GetBullet(spawnPos, fireRotation);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        // Green/Red Line of Sight line
        if (target)
        {
            var origin = eyePoint ? eyePoint.position : transform.position + Vector3.up * 1.5f;
            Gizmos.color = HasLineOfSight() ? Color.green : Color.red;
            Gizmos.DrawLine(origin, target.position);
        }

        // Blue Wire Sphere showing preferred shooting distance stopping point
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, preferredShootingDistance);
    }

    private void OnDestroy()
    {
        if (_bulletPickupPrefab)
        {
            var pos = new Vector3(transform.position.x, transform.position.y - 0.86f, transform.position.z);
            Instantiate(_bulletPickupPrefab, pos, Quaternion.identity);
        }
    }
}