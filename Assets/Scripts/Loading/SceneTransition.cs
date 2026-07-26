using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] protected Color lockedColor;
    [SerializeField] protected Color unlockedColor;
    
    [Header("Object References")]
    [SerializeField] protected Material panelMaterial;
    
    [Header("Current Scene References")]
    [SerializeField] protected Transform enemiesTransform;
    
    [Header("Scene Settings")]
    [SerializeField] protected string nextSceneName;
    [SerializeField] protected int nextBulletCap;
    [SerializeField] protected bool isStage;
    [SerializeField] protected bool isRound;

    private void Update()
    {
        if (enemiesTransform)
        {
            panelMaterial.color = enemiesTransform.childCount > 0 ? lockedColor : unlockedColor;
        }
        else
        {
            panelMaterial.color = unlockedColor;
        }
    }

    public virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(enemiesTransform)
                if(enemiesTransform.childCount > 0) return;

            if (isRound)
            {
                // send to shop the new scene and the on complete action
            }
            
            LoadingManager.Instance.LoadScene(nextSceneName, onComplete: () => 
            {
                CameraLook.Instance.LockCursor();
                PlayerDmgDealer.Instance.canShoot = true;
                PlayerDmgDealer.Instance.ForceStopShooting();
                
                LocomotionController.Instance.ResetPositionRotation(PlayerData.Instance.GetScenePosition(nextSceneName), 
                    PlayerData.Instance.GetSceneRotation(nextSceneName));
                
                if (isStage)
                {
                    LoseScreenController.Instance.currentStageSceneName = nextSceneName;
                    PlayerData.Instance.ClearNonPersistents();
                    BulletManager.Instance.InitializeNextStage(nextBulletCap);
                } 
                if (isRound)
                {
                    BulletManager.Instance.InitializeNextRound();
                }
            });
        }
    }
}
