using UnityEngine;

public class MeleeAttackState : EnemyState
{
    private readonly MeleeEnemyBrain _meleeBrain;
    private float _nextAttackTime;
    
    public MeleeAttackState(MeleeEnemyBrain meleeBrain) : base(meleeBrain)
    {
        _meleeBrain = meleeBrain;
    }

    public override void Enter()
    {
        base.Enter();
        _meleeBrain.StopPathfinding();

        _nextAttackTime = Time.time; // instantly attack
    }

    public override void Update()
    {
        base.Update();
        
        var dist = Vector3.Distance(_meleeBrain.transform.position, _meleeBrain.target.position);

        if (dist > _meleeBrain.meleeRange)
        {
            _meleeBrain.SwitchState(new MeleeChaseState(_meleeBrain));
            return;
        }

        if (Time.time >= _nextAttackTime)
        {
            _nextAttackTime = Time.time + _meleeBrain.attackCooldown;
            _meleeBrain.PerformMeleeSlash();
        }
    }
}
