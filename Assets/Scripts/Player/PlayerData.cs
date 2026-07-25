using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    [Header("Loadout & Inventory")]

    private readonly HashSet<string> _unlockedItemIDs = new();

    #region Properties

    [Header("Properties")]
    [field: SerializeField] public bool isPistolPiercing { get; private set; }
    [field: SerializeField] public bool isDoublePistolFireRate { get; private set; }
    [field: SerializeField] public bool isDoubleShotgunFireRate { get; private set; }
    [field: SerializeField] public bool isGroundSlamAttack { get; private set; }
    [field: SerializeField] public bool isDoubleDmgMul { get; private set; }
    [field: SerializeField] public bool isHalfDmgTaken { get; private set; }
    [field: SerializeField] public bool isPlus20BulletsPerRound { get; private set; }

    #endregion
    
    public bool TryBuyItem(ShopItemSO item)
    {
        if (_unlockedItemIDs.Contains(item.itemID)) 
            return false;
        
        if (!BulletManager.Instance.TryTakeBullets(item.price)) 
            return false;
        
        _unlockedItemIDs.Add(item.itemID);

        ApplyPurchasedItem(item.itemID);
        return true;
    }

    public bool TryRemoveItemByID(string itemID)
    {
        if (!_unlockedItemIDs.Remove(itemID))
        {
            return false;
        }

        UnapplyPurchasedItem(itemID);
        return true;
    }

    public void ClearAllItems()
    {
        foreach (var itemID in _unlockedItemIDs)
        {
            UnapplyPurchasedItem(itemID);
        }
        _unlockedItemIDs.Clear();
    }
    
    private void ApplyPurchasedItem(string itemID)
    {
        switch (itemID)
        {
            case "pistol_piercing":
                isPistolPiercing = true;
                break;
            case "2x_pistol_fire_rate":
                isDoublePistolFireRate = true;
                break;
            case "shotgun":
                // Equip and buy
                break;
            case "2x_shotgun_fire_rate":
                isDoubleShotgunFireRate = true;
                break;
            case "ground_slam_attack":
                isGroundSlamAttack = true;
                break;
            case "2x_dmg_mul":
                isDoubleDmgMul = true;
                break;
            case "05x_dmg_taken":
                isHalfDmgTaken = true;
                break;
            case "p20_bullets_per_round":
                isPlus20BulletsPerRound = true;
                break;
            default:
                Debug.LogWarning($"Unknown item ID: {itemID}");
                break;
        }
    }
    
    private void UnapplyPurchasedItem(string itemID)
    {
        switch (itemID)
        {
            case "pistol_piercing":
                isPistolPiercing = false;
                break;
            case "2x_pistol_fire_rate":
                isDoublePistolFireRate = false;
                break;
            case "shotgun":
                // Equip and buy
                break;
            case "2x_shotgun_fire_rate":
                isDoubleShotgunFireRate = false;
                break;
            case "ground_slam_attack":
                isGroundSlamAttack = false;
                break;
            case "2x_dmg_mul":
                isDoubleDmgMul = false;
                break;
            case "05x_dmg_taken":
                isHalfDmgTaken = false;
                break;
            case "p20_bullets_per_round":
                isPlus20BulletsPerRound = false;
                break;
            default:
                Debug.LogWarning($"Unknown item ID: {itemID}");
                break;
        }
    }
}