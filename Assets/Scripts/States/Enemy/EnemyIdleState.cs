public class EnemyIdleState : EnemyState
{
    private readonly EnemyState _nextStateOnSpotted;

    public EnemyIdleState(EnemyBrain enemy, EnemyState nextStateOnSpotted) : base(enemy)
    {
        _nextStateOnSpotted = nextStateOnSpotted;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.StopPathfinding();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.CanSeeTarget())
        {
            enemy.SwitchState(_nextStateOnSpotted);
        }
    }
}
