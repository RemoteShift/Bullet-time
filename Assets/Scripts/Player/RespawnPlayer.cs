using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private Transform respawnTransform;
    
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        BulletManager.Instance.ForceTakeBullets(15);
        
        LocomotionController.Instance.ResetPositionRotation(respawnTransform.position, respawnTransform.rotation);
    }
}
