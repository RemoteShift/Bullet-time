using UnityEngine;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject titleObject;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject quitButton;

    private void Start()
    {
        titleObject.transform.localScale = Vector3.zero;
        titleObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        
        playButton.transform.position = new Vector3(-Screen.width, playButton.transform.position.y, playButton.transform.position.z);
        playButton.transform.DOMoveX(Screen.width / 2f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f);
        
        var rectTransform = quitButton.GetComponent<RectTransform>();
        
        var initY = rectTransform.anchoredPosition.y;
        
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -Screen.height);
        
        rectTransform.DOAnchorPosY(initY, 1.5f).SetDelay(0.5f);
    }

    public void Play()
    {
        LoadingManager.Instance.LoadScene(/*PlayerData.Instance.finishedTutorial ? */"Game"/* : "Tutorial"*/, onComplete: () =>
        {
            CameraLook.Instance.LockCursor();
            PlayerData.Instance.ClearAll();
            BulletManager.Instance.InitializeNextStage(BulletManager.Instance.currentBulletCap);
            BulletManager.Instance.bulletDecEnabled = true;
            PlayerDmgDealer.Instance.canShoot = true;
            GameScreenController.ShowGameScreen();
        });
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
