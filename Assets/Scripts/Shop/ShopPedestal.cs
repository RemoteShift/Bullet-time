using TMPro;
using UnityEngine;
using DG.Tweening;

public class ShopPedestal : MonoBehaviour
{
    [SerializeField] private GameObject pedestalHighlight;
    [SerializeField] private Transform pedestalTransform;
    
    [Header("Item Config")]
    [SerializeField] private ShopItemSO itemData;
    [SerializeField] private Transform itemDisplayPoint;

    [Header("World Space UI")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;

    private GameObject _spawnedDisplayModel;
    private Tween _rotateTween;

    private void Start()
    {
        if (!itemData) return;
        
        if (PlayerData.Instance.IsAlreadyPurchased(itemData))
        {
            pedestalHighlight.SetActive(false);
            priceText.transform.parent.GetComponent<Canvas>().enabled = false;
        }

        SpawnDisplayModel();
        UpdatePedestalVisuals();
    }

    private void SpawnDisplayModel()
    {
        if (!itemData.displayPrefab || !itemDisplayPoint) return;

        _spawnedDisplayModel = Instantiate(itemData.displayPrefab, itemDisplayPoint);
        
        _rotateTween = _spawnedDisplayModel.transform.DORotate(new Vector3(0, 360, 0), 4f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }

    public void UpdatePedestalVisuals()
    {
        if (nameText) nameText.text = itemData.itemName;
        if (priceText) priceText.text = $"${itemData.price}";
    }
    
    public void InteractWithPedestal()
    {
        if (PlayerData.Instance.TryBuyItem(itemData))
        {
            AnimatePurchaseAndDespawn();
        }
        else
        {
            AnimateFail();
        }
    }

    private void AnimatePurchaseAndDespawn()
    {
        _rotateTween?.Kill();

        var buySeq = DOTween.Sequence();
        
        if (_spawnedDisplayModel)
        {
            buySeq.Append(_spawnedDisplayModel.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack));
        }
        
        if (pedestalHighlight)
        {
            pedestalHighlight.SetActive(false);
        }
        
        buySeq.OnComplete(() => 
        {
            if (itemDisplayPoint)
            {
                Destroy(itemDisplayPoint.gameObject);
            }
        });
    }

    private void AnimateFail()
    {
        pedestalTransform.DOShakePosition(0.3f, strength: new Vector3(0.1f, 0, 0), vibrato: 20);
    }

    private void OnDestroy()
    {
        _rotateTween?.Kill();
    }
}