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

    private int _lastBulletIndex;

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

    private void UpdateBulletText(int currentBullets)
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
    }

    private void ClearIcons()
    {
        bulletIcons.Clear();
        foreach (Transform child in bulletIconsContentTransform)
        {
            Destroy(child.gameObject);
        }
    }
    
    private void UpdateBulletIcons(int deltaBullets)
    {
        if (deltaBullets < 0)
        {
            for(var i = 0; i < Mathf.Abs(deltaBullets); i++)
            {
                var index = _lastBulletIndex - i;
                if (index < 0 || index >= bulletIcons.Count) continue;
                bulletIcons[_lastBulletIndex - i].GetComponent<RawImage>().color = Color.black;
            }
            _lastBulletIndex = Math.Clamp(_lastBulletIndex + deltaBullets, 0, bulletIcons.Count - 1);
        }
        else if (deltaBullets > 0)
        {
            for (var i = 0; i < deltaBullets; i++)
            {
                var index =  _lastBulletIndex + i;
                if (index < 0 || index >= bulletIcons.Count) continue;
                bulletIcons[_lastBulletIndex + i].GetComponent<RawImage>().color = Color.white;
            }
            _lastBulletIndex = Math.Clamp(_lastBulletIndex + deltaBullets, 0, bulletIcons.Count - 1);
        }
    }
}
