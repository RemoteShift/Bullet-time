using UnityEngine;

public class MageAttackState : EnemyState
{
    private readonly MageEnemyBrain _mageBrain;

    private float _endTime;
    
    public MageAttackState(MageEnemyBrain enemy) : base(enemy)
    {
        _mageBrain = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        _mageBrain.StopPathfinding();
        _mageBrain.CastSpell();
    }

    public override void Update()
    {
        base.Update();
        
        if(!_mageBrain.CanSeeTarget())
            _mageBrain.ReturnToDefaultState();
    }

    public override void Exit()
    {
        base.Exit();
        _mageBrain.DecastSpell();
    }
}
