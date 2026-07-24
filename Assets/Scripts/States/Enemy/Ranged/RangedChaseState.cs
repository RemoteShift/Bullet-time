using UnityEngine;

public class RangedChaseState : EnemyState
{
    private readonly RangedEnemyBrain _rangedBrain;

    public RangedChaseState(RangedEnemyBrain enemy) : base(enemy)
    {
        _rangedBrain = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        _rangedBrain.PathfindToTarget();
    }

    public override void Update()
    {
        if (!_rangedBrain.target)
        {
            _rangedBrain.ReturnToDefaultState();
            return;
        }
        
        _rangedBrain.PathfindToTarget();
        
        var distanceToPlayer = Vector3.Distance(_rangedBrain.transform.position, _rangedBrain.target.position);
        var inShootingRange = distanceToPlayer <= _rangedBrain.preferredShootingDistance;
        var hasClearSight = _rangedBrain.HasLineOfSight();

        if (inShootingRange && hasClearSight)
        {
            _rangedBrain.SwitchState(new RangedAttackState(_rangedBrain));
        }
    }
}