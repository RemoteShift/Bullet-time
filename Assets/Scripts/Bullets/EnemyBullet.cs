using UnityEngine;
using UnityEngine.Pool;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float damage = 2f;
    [SerializeField] private float maxLifetime = 4f;

    private IObjectPool<EnemyBullet> _pool;
    private float _spawnTime;

    public void SetPool(IObjectPool<EnemyBullet> pool)
    {
        _pool = pool;
    }

    private void OnEnable()
    {
        _spawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        var moveDistance = speed * Time.fixedDeltaTime;
        
        if (Physics.Raycast(transform.position, transform.forward, out var hit, moveDistance))
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(damage);
            }
            
            _pool.Release(this);
            return;
        }

        transform.Translate(Vector3.forward * moveDistance);

        if (Time.time >= _spawnTime + maxLifetime)
        {
            _pool.Release(this);
        }
    }
}