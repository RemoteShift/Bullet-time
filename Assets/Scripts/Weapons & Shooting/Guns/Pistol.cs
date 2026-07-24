using UnityEngine;

public class Pistol : BaseGun
{
    [Header("Pistol Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    
    protected override void FirePattern()
    {
        var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        DealInfinitePiercingDamage(ray, range, damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * range);
    }
}
