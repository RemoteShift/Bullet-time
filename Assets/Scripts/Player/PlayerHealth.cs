using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage)
    {
        var damageInBullets = Mathf.RoundToInt(damage);
        
        BulletManager.Instance.ForceTakeBullets(damageInBullets);

        OnPlayerHurt(damageInBullets);
    }

    public void Stun()
    {
        throw new System.NotImplementedException();
    }

    private void OnPlayerHurt(int bulletsLost)
    {
        Debug.Log($"Player hit! Lost {bulletsLost} bullets");
    }

    public void Die()
    {
        // Play death screen
    }
}
