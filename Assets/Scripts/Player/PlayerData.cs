using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    [Header("Loadout & Inventory")]

    private readonly HashSet<string> _unlockedItemIDs = new();

    #region Shop Properties

    [Header("Shop Properties")]
    [field: SerializeField] public bool isPistolPiercing { get; private set; }
    [field: SerializeField] public bool isDoublePistolFireRate { get; private set; }
    [field: SerializeField] public bool isDoubleShotgunFireRate { get; private set; }
    [field: SerializeField] public bool isGroundSlamAttack { get; private set; }
    [field: SerializeField] public bool isDoubleDmgMul { get; private set; }
    [field: SerializeField] public bool isHalfDmgTaken { get; private set; }
    [field: SerializeField] public bool isPlus20BulletsPerRound { get; private set; }

    #endregion

    #region Properties
    
    [Header("Properties")]
    public bool isTutorialCompleted;

    #endregion

    #region Scene Start Positions

    [Header("Scene Start Positions")]
    [SerializeField] private Vector3 tutorialStartPosition;
    [SerializeField] private Vector3 gameStartPosition;
    [SerializeField] private Vector3 shopStartPosition;

    #endregion

    #region Scene Start Rotations

    [Header("Scene Start Rotations")]
    [SerializeField] private Vector3 tutorialStartRotation;
    [SerializeField] private Vector3 gameStartRotation;
    [SerializeField] private Vector3 shopStartRotation;

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

    public void ClearNonPersistents()
    {
        foreach (var itemID in _unlockedItemIDs)
        {
            if(itemID.Equals("shotgun")) continue; // Keep shotgun unlocked
            UnapplyPurchasedItem(itemID);
            _unlockedItemIDs.Remove(itemID);
        }
    }

    public void ClearAll()
    {
        foreach (var itemID in _unlockedItemIDs)
        {
            UnapplyPurchasedItem(itemID);
        }
        _unlockedItemIDs.Clear();
    }

    public Vector3 GetScenePosition(string sceneName)
    {
        switch (sceneName)
        {
            case "Tutorial":
                return tutorialStartPosition;
            case "Game":
                return gameStartPosition;
            case "Shop":
                return shopStartPosition;
            default:
                Debug.LogWarning($"Unknown scene name: {sceneName}. Returning Vector3.zero.");
                return Vector3.zero;
        }
    }

    public Vector3 GetSceneRotation(string sceneName)
    {
        switch (sceneName)
        {
            case "Tutorial":
                return tutorialStartRotation;
            case "Game":
                return gameStartRotation;
            case "Shop":
                return shopStartRotation;
            default:
                Debug.LogWarning($"Unknown scene name: {sceneName}. Returning Vector3.zero.");
                return Vector3.zero;
        }
    }

    public bool IsAlreadyPurchased(ShopItemSO item)
    {
        return _unlockedItemIDs.Contains(item.itemID);
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
                PlayerDmgDealer.Instance.AddShotgun();
                PlayerDmgDealer.Instance.EquipGun(1); // temporary setter
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
                // Never Unapply anyway
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