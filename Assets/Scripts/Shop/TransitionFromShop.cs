using UnityEngine;

public class TransitionFromShop : SceneTransition
{
    private void Start()
    {
        nextSceneName = ShopTransitionHandler.Instance.sceneName;
        nextBulletCap = ShopTransitionHandler.Instance.nextBulletCap;
    }

    public override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            LoadingManager.Instance.LoadScene(nextSceneName, onComplete: ShopTransitionHandler.Instance.onComplete);
        }
    }
}
