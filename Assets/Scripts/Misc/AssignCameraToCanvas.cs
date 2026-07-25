using UnityEngine;

public class AssignCameraToCanvas : MonoBehaviour
{
    void Start()
    {
        if(Camera.main)
            GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
