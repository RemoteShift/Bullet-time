using UnityEngine;

public class GameScreenController : Singleton<GameScreenController>
{
    public static void ShowGameScreen()
    {
        var canvas = Instance.GetComponent<Canvas>();
        if (canvas.enabled) return;
        
        canvas.enabled = true;
    }

    public static void HideGameScreen()
    {
        var canvas = Instance.GetComponent<Canvas>();
        if (!canvas.enabled) return;
        
        canvas.enabled = false;
    }
}
