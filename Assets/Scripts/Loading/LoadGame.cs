using UnityEngine;
public class LoadGame : MonoBehaviour
{
    void Awake()
    {
        LoadingManager.Instance.LoadScene("Main Menu", onComplete: () =>
        {
            CameraLook.Instance.UnlockCursor();
        });
    }
}
