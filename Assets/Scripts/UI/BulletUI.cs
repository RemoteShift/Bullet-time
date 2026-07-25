using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BulletUI : Singleton<BulletUI>
{
    [SerializeField] private TextMeshProUGUI bulletText;
    [SerializeField] private TextMeshProUGUI extraBulletText;

    [FormerlySerializedAs("_bulletIconPrefab")] [SerializeField] private GameObject bulletIconPrefab;
    [FormerlySerializedAs("_bulletIcons")] [SerializeField] private List<GameObject> bulletIcons;
    [FormerlySerializedAs("_bulletIconsContentTransform")] [SerializeField] private Transform bulletIconsContentTransform;

    [SerializeField] private int lastBulletIndex;

    private void OnEnable()
    {
        BulletManager.Instance.OnBulletCountChanged.AddListener(UpdateBulletText);
        BulletManager.Instance.OnBulletCountChangedDelta.AddListener(UpdateBulletIcons);
    }

    private void OnDisable()
    {
        BulletManager.Instance.OnBulletCountChanged.RemoveListener(UpdateBulletText);
        BulletManager.Instance.OnBulletCountChangedDelta.RemoveListener(UpdateBulletIcons);
    }

    private void Start()
    {
        UpdateBulletText(BulletManager.Instance.currentBullets);
        UpdateBulletIcons(BulletManager.Instance.currentBullets);
    }

    public void UpdateBulletText(int currentBullets)
    {
        if (currentBullets > BulletManager.Instance.currentBulletCap)
        {
            bulletText.text = BulletManager.Instance.currentBulletCap.ToString();
            extraBulletText.text = $"+{currentBullets - BulletManager.Instance.currentBulletCap}";
            return;
        }
        bulletText.text = currentBullets.ToString();
        extraBulletText.text = "";
    }

    public void PopulateBulletIcons(int bulletCap)
    {
        ClearIcons();
        
        for (var i = 0; i < bulletCap; i++)
        {
            var icon = Instantiate(bulletIconPrefab, bulletIconsContentTransform);
            bulletIcons.Add(icon);
        }
        
        lastBulletIndex = bulletCap - 1;
    }

    private void ClearIcons()
    {
        bulletIcons.Clear();
        foreach (Transform child in bulletIconsContentTransform)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void UpdateBulletIcons(int deltaBullets)
    {
        var initLastBulletIndex = lastBulletIndex;
        if (deltaBullets < 0)
        {
            var skippedBullets = 0;
            
            for(var i = 0; i < Math.Abs(deltaBullets); i++)
            {
                if (BulletManager.Instance.currentBullets + Math.Abs(deltaBullets) - i >
                    BulletManager.Instance.currentBulletCap)
                {
                    skippedBullets++;
                    continue;
                }
                
                var index = lastBulletIndex - (i - skippedBullets);
                if (index < 0 || index >= bulletIcons.Count) continue;
                bulletIcons[index].GetComponent<RawImage>().color = Color.black;
                initLastBulletIndex--;
            }

            lastBulletIndex = initLastBulletIndex;
        }
        else if (deltaBullets > 0)
        {
            for (var i = 0; i < deltaBullets; i++)
            {
                var index =  lastBulletIndex + i;
                if (index < 0 || index >= bulletIcons.Count) continue;
                bulletIcons[index].GetComponent<RawImage>().color = Color.white;

                if (BulletManager.Instance.currentBullets - deltaBullets + i < BulletManager.Instance.currentBulletCap)
                {
                    initLastBulletIndex++;
                }
            }

            lastBulletIndex = initLastBulletIndex;
        }
    }
}
