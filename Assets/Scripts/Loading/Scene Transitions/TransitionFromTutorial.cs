using UnityEngine;

public class TransitionFromTutorial : SceneTransition
{
    public override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(enemiesTransform)
                if(enemiesTransform.childCount > 0) return;
            
            PlayerData.Instance.isTutorialCompleted = true;
        }
    }
}
