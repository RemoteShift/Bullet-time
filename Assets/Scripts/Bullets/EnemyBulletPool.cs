using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool : Singleton<EnemyBulletPool>
{
    [Header("Pool Config")]
    [SerializeField] private EnemyBullet bulletPrefab;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxPoolSize = 100;

    private IObjectPool<EnemyBullet> _pool;

    protected override void Awake()
    {
        base.Awake();
        
        _pool = new ObjectPool<EnemyBullet>(
            createFunc: CreateBullet,
            actionOnGet: OnGetBullet,
            actionOnRelease: OnReleaseBullet,
            actionOnDestroy: OnDestroyBullet,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxPoolSize
        );
    }
    
    private EnemyBullet CreateBullet()
    {
        var bullet = Instantiate(bulletPrefab, transform);
        bullet.SetPool(_pool);
        return bullet;
    }
    
    private void OnGetBullet(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    
    private void OnReleaseBullet(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    
    private void OnDestroyBullet(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }
    
    public EnemyBullet GetBullet(Vector3 position, Quaternion rotation)
    {
        var bullet = _pool.Get();
        bullet.transform.SetPositionAndRotation(position, rotation);
        return bullet;
    }
}