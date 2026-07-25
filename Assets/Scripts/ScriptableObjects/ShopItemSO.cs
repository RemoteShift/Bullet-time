using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/Shop Item")]
public class ShopItemSO : ScriptableObject
{
    public string itemID;
    public string itemName;
    public ShopItemType itemType;
    public int price;
    public GameObject displayPrefab;
}