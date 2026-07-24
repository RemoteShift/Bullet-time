using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBrain : MonoBehaviour, IDamageable
{
    private EnemyState currentState;
    public EnemyState CurrentState => currentState;
    
    [SerializeField] private string currentStateName;
    
    [Header("Base References")]
    public NavMeshAgent agent;
    public Transform target;
    
    [Header("Base Stats & Detection")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float detectRange = 25f;
    public float attackCooldown = 1.5f;

    [Header("Pathfinding Settings")] [SerializeField]
    private float repathInterval = 0.15f;
    private float _nextRepathTime;
    
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
        ReturnToDefaultState();
    }

    protected void Update()
    {
        currentState?.Update();
    }

    protected void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    #region State Machine Logic

    public void SwitchState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();

        currentStateName = currentState != null ? currentState.GetType().Name : "None";
    }
    
    public abstract void ReturnToDefaultState();

    #endregion

    #region Shared helper states

    public virtual bool CanSeeTarget()
    {
        if (!target) return false;
        return Vector3.Distance(transform.position, target.position) <= detectRange;
    }

    public virtual void PathfindToTarget()
    {
        if (!target || !agent.isActiveAndEnabled) return;

        agent.isStopped = false;

        if (Time.time >= _nextRepathTime)
        {
            _nextRepathTime = Time.time + repathInterval;
            agent.SetDestination(target.position);
        }
    }

    public virtual void StopPathfinding()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }
    }

    public virtual void LookAtTarget()
    {
        if (!target) return;
        
        var dir = target.position - transform.position;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 
                Clock.Instance.DeltaTime * 10f);
        }
    }

    #endregion

    #region IDamageable Implementation

    public virtual void TakeDamage(float damage)
    {
        Debug.Log("Enemy took damage: " + damage);
        currentHealth -= damage;

        StartCoroutine(BlinkRed());
        
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
    
    #endregion

    #region Coroutines

    private IEnumerator BlinkRed()
    {
        var meshRend = GetComponent<Renderer>();
        meshRend.material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        meshRend.material.color = Color.white;
    }

    #endregion
    
    #region Gizmos

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    #endregion
}
