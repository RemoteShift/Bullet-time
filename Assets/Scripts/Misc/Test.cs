using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            LoadingManager.Instance.LoadScene("Shop", onComplete: () =>
            {
                BulletManager.Instance.bulletDecEnabled = false;
                PlayerDmgDealer.Instance.canShoot = false;
            });
        }
    }
}
