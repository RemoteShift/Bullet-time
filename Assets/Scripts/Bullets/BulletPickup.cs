using UnityEngine;
using DG.Tweening;

public class BulletPickup : MonoBehaviour
{
    [SerializeField] private int _bulletAmount = 5;
    
    [Header("Animation Settings")]
    [SerializeField] private float floatDistance = 0.2f;
    [SerializeField] private float floatDuration = 0.2f;

    private void OnEnable()
    {
        transform.DOMoveY(transform.position.y + floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            BulletManager.Instance.ForceAddBullets(_bulletAmount);
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        if (Camera.main)
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
