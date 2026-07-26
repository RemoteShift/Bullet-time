using UnityEngine;
using DG.Tweening;

public class LoseScreenController : Singleton<LoseScreenController>
{
    [Header("References")]
    [SerializeField] private GameObject youLoseText;
    [SerializeField] private GameObject mainMenuButton;

    private void OnCanvasEnable()
    {
        CameraLook.Instance.UnlockCursor();
        PlayerDmgDealer.Instance.canShoot = false;
        BulletManager.Instance.bulletDecEnabled = false;
        
        youLoseText.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f, 10, 1);
        
        var rectTransform = mainMenuButton.GetComponent<RectTransform>();
        
        var initY = rectTransform.anchoredPosition.y;
        
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -Screen.height);
        
        rectTransform.DOAnchorPosY(initY, 1.5f).SetDelay(0.5f);
    }

    public void Restart()
    {
        HideLoseScreen();
        LoadingManager.Instance.LoadScene(/*PlayerData.Instance.finishedTutorial ? */"Game"/* : "Tutorial"*/, onComplete: () =>
        {
            PlayerData.Instance.ClearAll();
            BulletManager.Instance.InitializeNextStage(BulletManager.Instance.currentBulletCap);
            BulletManager.Instance.bulletDecEnabled = true;
            PlayerDmgDealer.Instance.canShoot = true;
            CameraLook.Instance.LockCursor();
            GameScreenController.ShowGameScreen();
        });
    }
    
    public void MainMenu()
    {
        HideLoseScreen();
        LoadingManager.Instance.LoadScene("Main Menu", onComplete: () =>
        {
            CameraLook.Instance.UnlockCursor();
            PlayerData.Instance.ClearAll();
            BulletManager.Instance.bulletDecEnabled = false;
            PlayerDmgDealer.Instance.canShoot = false;
        });
    }
    
    public static void ShowLoseScreen()
    {
        var canvas = Instance.GetComponent<Canvas>();

        if (canvas.enabled) return;
        
        GameScreenController.HideGameScreen();
        canvas.enabled = true;
        Instance.OnCanvasEnable();
    }

    public static void HideLoseScreen()
    {
        var canvas = Instance.GetComponent<Canvas>();
        if (!canvas.enabled) return;
        
        canvas.enabled = false;
    }
}
