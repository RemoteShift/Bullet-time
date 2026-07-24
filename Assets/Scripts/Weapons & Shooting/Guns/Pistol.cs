using UnityEngine;

public class Pistol : BaseGun
{
    [Header("Pistol Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    
    protected override void FirePattern()
    {
        var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out var hit, range))
        {
            if(hit.collider.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * range);
    }
}
