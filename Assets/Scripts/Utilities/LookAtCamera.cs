using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Camera.main)
        {
            var dir = Camera.main.transform.position - transform.position;

            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 
                    Clock.Instance.DeltaTime * 10f);
            }
        }
    }
}
