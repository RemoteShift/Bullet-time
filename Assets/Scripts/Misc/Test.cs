using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            BulletManager.Instance.bulletDecEnabled = false;
            PlayerDmgDealer.Instance.ForceStopShooting();
            PlayerDmgDealer.Instance.canShoot = false;
            LoadingManager.Instance.LoadScene("Shop", onComplete: () =>
            {
                
            });
        }
    }
}
