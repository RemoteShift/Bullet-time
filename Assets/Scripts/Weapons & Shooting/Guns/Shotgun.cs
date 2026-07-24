using UnityEngine;

public class Shotgun : BaseGun
{
    [Header("Shotgun Settings")]
    [SerializeField] private int pelletCount = 8;
    [SerializeField] private float damagePerPellet = 10f;
    [SerializeField] private float range = 30f;
    [SerializeField] private float spreadAngle = 6f;

    protected override void FirePattern()
    {
        var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        for (var i = 0; i < pelletCount; i++)
        {
            var targetDirection = playerCamera.transform.forward;
            var randomSpread = Random.insideUnitCircle * Mathf.Tan(spreadAngle * Mathf.Deg2Rad);
            
            targetDirection += playerCamera.transform.right * randomSpread.x;
            targetDirection += playerCamera.transform.up * randomSpread.y;
            targetDirection.Normalize();

            var spreadRay = new Ray(ray.origin, targetDirection);

            if (TryHitScan(spreadRay, range, out var hit))
            {
                if (TryGetDamageable(hit.collider, out var target))
                {
                    target.TakeDamage(damagePerPellet);
                }
            }
        }
    }
}
