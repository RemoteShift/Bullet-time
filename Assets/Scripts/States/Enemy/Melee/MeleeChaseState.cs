using UnityEngine;

public class MeleeChaseState : EnemyState
{
    private readonly MeleeEnemyBrain _meleeBrain;

    public MeleeChaseState(MeleeEnemyBrain meleeBrain) : base(meleeBrain)
    {
        _meleeBrain = meleeBrain;
    }

    public override void Update()
    {
        base.Update();
        
        _meleeBrain.PathfindToTarget();
        
        var dist = Vector3.Distance(_meleeBrain.transform.position, _meleeBrain.target.position);

        if (dist <= _meleeBrain.meleeRange)
        {
            _meleeBrain.SwitchState(new MeleeAttackState(_meleeBrain));
        }
    }
}
