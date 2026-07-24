using UnityEngine;

public class RangedAttackState : EnemyState
{
    private readonly RangedEnemyBrain _rangedBrain;

    public RangedAttackState(RangedEnemyBrain enemy) : base(enemy)
    {
        _rangedBrain = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        _rangedBrain.StopPathfinding();
    }

    public override void Update()
    {
        base.Update();
        
        var distToPlayer = Vector3.Distance(_rangedBrain.transform.position, _rangedBrain.target.position);
        var lineOfSightLost = !_rangedBrain.HasLineOfSight();
        var outOfRange = distToPlayer > _rangedBrain.preferredShootingDistance + 2f;

        if (lineOfSightLost || outOfRange)
        {
            _rangedBrain.SwitchState(new RangedChaseState(_rangedBrain));
            return;
        }
        
        if (Time.time >= _rangedBrain.nextAttackTime)
        {
            _rangedBrain.nextAttackTime = Time.time + _rangedBrain.attackCooldown;
            _rangedBrain.FireProjectile();
        }
    }
}