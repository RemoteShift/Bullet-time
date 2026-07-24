using UnityEngine;

public class MeleeEnemyBrain : EnemyBrain
{
    [Header("Melee Specifics")]
    public float meleeRange = 2.5f;
    public float meleeDamage = 5f;
    // public Transform attackPoint;

    public override void ReturnToDefaultState()
    {
        SwitchState(new EnemyIdleState(this, new MeleeChaseState(this)));
    }

    public void PerformMeleeSlash()
    {
        if (!target) return;
        
        if (target.position.y > transform.position.y) return;

        if (Vector3.Distance(transform.position, target.position) <= meleeRange)
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(meleeDamage);
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
