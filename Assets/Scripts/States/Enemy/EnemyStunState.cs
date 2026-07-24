using UnityEngine;

public class EnemyStunState : EnemyState
{
    private float _stunDuration;
    private float _stunTimer;

    public EnemyStunState(EnemyBrain enemy, float stunDuration) : base(enemy)
    {
        _stunDuration = stunDuration;
    }

    public override void Enter()
    {
        base.Enter();
        _stunTimer = 0f;
        enemy.StopPathfinding();
    }

    public override void Update()
    {
        base.Update();
        _stunTimer += Time.deltaTime;

        if (_stunTimer >= _stunDuration)
        {
            enemy.ReturnToDefaultState();
        }
    }
}
