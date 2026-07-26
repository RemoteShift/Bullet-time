using UnityEngine;
using DG.Tweening;

public class Bounce : MonoBehaviour
{
    [SerializeField] private float floatDistance;
    [SerializeField] private float floatDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.DOMoveY(transform.position.y + floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
