using UnityEngine;

public class Test1 : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            LoadingManager.Instance.LoadScene("Game", onComplete: () =>
            {
                BulletManager.Instance.Initialize(BulletManager.Instance.currentBullets);
                BulletManager.Instance.bulletDecEnabled = true;
                PlayerDmgDealer.Instance.canShoot = true;
            });
        }
    }
}
