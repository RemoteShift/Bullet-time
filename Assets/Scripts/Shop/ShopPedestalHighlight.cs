using UnityEngine;

public class ShopPedestalHighlight : MonoBehaviour
{
    [SerializeField] private Canvas interactCanvas;
    [SerializeField] private ShopPedestal shopPedestal;
    
    private void OnTriggerEnter(Collider other)
    {
        if (interactCanvas) interactCanvas.enabled = true;
        if (InputHandler.Instance)
        {
            InputHandler.Instance.OnInteractInput += HandleInteract;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactCanvas) interactCanvas.enabled = false;
        if (InputHandler.Instance)
        {
            InputHandler.Instance.OnInteractInput -= HandleInteract;
        }
    }

    private void HandleInteract()
    {
        if (shopPedestal)
        {
            shopPedestal.InteractWithPedestal();
        }
    }

    private void OnDestroy()
    {
        if (InputHandler.Instance)
        {
            InputHandler.Instance.OnInteractInput -= HandleInteract;
        }
    }
}
