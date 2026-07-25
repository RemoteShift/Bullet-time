using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Camera.main)
        {
            // Face AWAY from the camera so the UI front (+Z) points toward the player
            var dir = transform.position - Camera.main.transform.position;

            if (dir != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    targetRotation, 
                    Clock.Instance.DeltaTime * 10f
                );
            }
        }
    }
}