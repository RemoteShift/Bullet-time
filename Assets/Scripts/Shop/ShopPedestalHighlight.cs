using UnityEngine;

public class ShopPedestalHighlight : MonoBehaviour
{
    [SerializeField] private Canvas interactCanvas;
    [SerializeField] private ShopPedestal shopPedestal;
    
    private void OnTriggerEnter(Collider other)
    {
        interactCanvas.enabled = true;
        InputHandler.Instance.OnInteractInput += HandleInteract;
    }

    private void OnTriggerExit(Collider other)
    {
        interactCanvas.enabled = false;
        InputHandler.Instance.OnInteractInput -= HandleInteract;
    }

    private void HandleInteract()
    {
        shopPedestal.InteractWithPedestal();
    }
}
