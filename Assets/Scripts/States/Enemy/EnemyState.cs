using UnityEngine;

public class EnemyState : IState
{
    protected readonly EnemyBrain enemy;
    
    public EnemyState(EnemyBrain enemy)
    {
        this.enemy = enemy;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }
}
