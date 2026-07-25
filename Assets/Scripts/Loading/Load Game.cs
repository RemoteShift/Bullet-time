using UnityEngine;
public class LoadGame : MonoBehaviour
{
    void Awake()
    {
        LoadingManager.Instance.LoadScene("Game", onComplete: () =>
        {
            BulletManager.Instance.InitializeNextStage(BulletManager.Instance.currentBulletCap);
            BulletManager.Instance.bulletDecEnabled = true;
            PlayerDmgDealer.Instance.canShoot = true;
        });
    }
}
