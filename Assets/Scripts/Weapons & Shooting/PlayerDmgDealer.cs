using System.Collections.Generic;
using UnityEngine;

public class PlayerDmgDealer : Singleton<PlayerDmgDealer>
{
    [Header("Gun Settings")]
    [SerializeField] private BaseGun currentGun;
    public bool canShoot;
    [SerializeField] private bool isAutomatic = true;

    private bool _isShooting;
    
    [Header("Slam Settings")]
    [SerializeField] private Transform slamPoint;
    [SerializeField] private float slamDamage = 10f;
    [SerializeField] private float slamRadius = 5f;
    [SerializeField] private LayerMask slamHitLayers;
    [SerializeField] private float slamKnockbackDistance = 2f;

    private void OnEnable()
    {
        InputHandler.Instance.OnLeftClickInput += HandleShootingInput;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnLeftClickInput -= HandleShootingInput;
    }

    private void Update()
    {
        if (!currentGun) return;
        
        switch (_isShooting)
        {
            case true when !isAutomatic:
                currentGun.TryShoot();
                _isShooting = false;
                break;
            case true when isAutomatic:
                currentGun.TryShoot();
                break;
        }
    }

    public void EquipGun(BaseGun newGun, bool automaticSetting)
    {
        if (currentGun)
        {
            currentGun.gameObject.SetActive(false);
        }

        currentGun = newGun;
        isAutomatic = automaticSetting;
        currentGun.gameObject.SetActive(true);
    }

    public void ForceStopShooting()
    {
        _isShooting = false;
    }
    
    private void HandleShootingInput(bool performed)
    {
        if (!currentGun || !canShoot) return;

        _isShooting = performed;
    }

    public void DealSlamDamage()
    {
        var layers = slamHitLayers.value == 0 ? ~0 : slamHitLayers.value;
        var colliders = Physics.OverlapSphere(slamPoint.position, slamRadius, layers, QueryTriggerInteraction.Ignore);
        var damagedTargets = new HashSet<IDamageable>();

        for (var i = 0; i < colliders.Length; i++)
        {
            if (!TryGetDamageable(colliders[i], out var damageable))
            {
                continue;
            }

            if (damagedTargets.Add(damageable))
            {
                damageable.Stun();
                damageable.TakeDamage(PlayerData.Instance.isDoubleDmgMul ? slamDamage * 2f : slamDamage);

                if (damageable is EnemyBrain enemyBrain)
                {
                    enemyBrain.KnockbackFrom(slamPoint.position, slamKnockbackDistance);
                }
            }
        }
    }

    private bool TryGetDamageable(Collider collider, out IDamageable damageable)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(slamPoint.position, slamRadius);
    }
}
