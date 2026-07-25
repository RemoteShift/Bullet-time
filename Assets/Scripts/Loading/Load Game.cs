using UnityEngine;
public class LoadGame : MonoBehaviour
{
    void Awake()
    {
        LoadingManager.Instance.LoadScene("Game");
    }
}
