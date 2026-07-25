using System;
using UnityEngine;
using UnityEngine.Events;

public class BulletManager : Singleton<BulletManager>
{
    [SerializeField] private int initialBullets = 50;
    public bool bulletDecEnabled = true;
    
    public int currentBullets;
    public int currentBulletCap = 50;
    public int bulletIncPerRound = 20;
    private float _bulletTimer;
    
    public UnityEvent<int> OnBulletCountChangedDelta;
    public UnityEvent<int> OnBulletCountChanged;

    public void InitializeNextRound(int extraBullets = 0)
    {
        currentBullets = bulletIncPerRound + extraBullets + (PlayerData.Instance.isPlus20BulletsPerRound ? 20 : 0) ;
        _bulletTimer = 0f;
        BulletUI.Instance.PopulateBulletIcons(currentBulletCap);
        BulletUI.Instance.UpdateBulletText(currentBullets);
    }

    public void InitializeNextStage(int bulletCap)
    {
        currentBulletCap = bulletCap;
        currentBullets = currentBulletCap;
        _bulletTimer = 0f;
        BulletUI.Instance.PopulateBulletIcons(currentBulletCap);
        BulletUI.Instance.UpdateBulletText(currentBullets);
    }

    private void FixedUpdate()
    {
        if (currentBullets > 0)
        {
            if (!bulletDecEnabled) return;
            _bulletTimer += Clock.Instance.FixedDeltaTime;
            if (_bulletTimer >= 1f)
            {
                _bulletTimer -= 1f;
                currentBullets--;
                OnBulletCountChangedDelta?.Invoke(-1);
                OnBulletCountChanged?.Invoke(currentBullets);
            }
        }
        else
        {
            Debug.Log("Out of bullets! YOU LOSE!");
        }
    }

    public bool TryTakeBullets(int amount)
    {
        if (currentBullets < amount)
        {
            return false;
        }

        currentBullets -= amount;
        OnBulletCountChangedDelta?.Invoke(-amount);
        OnBulletCountChanged?.Invoke(currentBullets);
        return true;
    }

    public bool ForceTakeBullets(int amount)
    {
        var initBullets = currentBullets;
        currentBullets = Math.Max(currentBullets - amount, 0);
        OnBulletCountChangedDelta?.Invoke(-(initBullets - currentBullets));
        OnBulletCountChanged?.Invoke(currentBullets);
        return true;
    }

    public void ForceAddBullets(int amount)
    {
        currentBullets += amount;
        OnBulletCountChanged?.Invoke(currentBullets);
        OnBulletCountChangedDelta?.Invoke(amount);
        return;
    }
}
