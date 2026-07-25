using UnityEngine;
public class LoadGame : MonoBehaviour
{
    void Start()
    {
        LoadingManager.Instance.LoadScene("Game");
    }
}
