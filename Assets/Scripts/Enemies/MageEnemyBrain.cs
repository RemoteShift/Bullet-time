using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MageEnemyBrain : EnemyBrain
{
    [Header("Spell Settings")]
    [SerializeField] private float bulletDecRate = 1f;

    [Header("Animation Settings")]
    [SerializeField] private Renderer renderer;
    [SerializeField] private float floatDistance = 0.5f;
    [SerializeField] private float floatDuration = 0.6f;
    
    [Header("SFX")]
    public AudioClip mageAttack;

    private Coroutine _attackCoroutine;
    private Tween _floatTween;

    public override void ReturnToDefaultState()
    {
        SwitchState(new EnemyIdleState(this, new MageAttackState(this)));
    }

    public void CastSpell()
    {
        enemyAudio.PlayEnemySound(mageAttack, loop: true, volumeMult: 5f);
        
        _attackCoroutine = StartCoroutine(CastSpellCoroutine());
        
        
        renderer.material.DOColor(Color.purple, 0.2f);
        
        _floatTween = transform.DOMoveY(transform.position.y + floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void DecastSpell()
    {
        enemyAudio.StopEnemySound();
        
        if (_attackCoroutine != null)
            StopCoroutine(_attackCoroutine);
        
        if (_floatTween != null && _floatTween.IsActive())
        {
            _floatTween.Kill();
        }

        renderer.material.DOColor(Color.white, 0.2f);
    }

    private IEnumerator CastSpellCoroutine()
    {
        while (true)
        {
            BulletManager.Instance.ForceTakeBullets(Mathf.RoundToInt(bulletDecRate));
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnDestroy()
    {
        _floatTween?.Kill();
    }
}