using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private BaseGun currentGun;

    [SerializeField] private bool isAutomatic = true;

    private bool _isShooting;

    private void OnEnable()
    {
        InputHandler.Instance.OnLeftClickInput += HandleShootingInput;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnLeftClickInput -= HandleShootingInput;
    }

    private void Update()
    {
        if (!currentGun) return;

        switch (_isShooting)
        {
            case true when !isAutomatic:
                currentGun.TryShoot();
                _isShooting = false;
                break;
            case true when isAutomatic:
                currentGun.TryShoot();
                break;
        }
    }

    public void EquipGun(BaseGun newGun, bool automaticSetting)
    {
        if (currentGun)
        {
            currentGun.gameObject.SetActive(false);
        }

        currentGun = newGun;
        isAutomatic = automaticSetting;
        currentGun.gameObject.SetActive(true);
    }
    
    private void HandleShootingInput(bool performed)
    {
        if (!currentGun) return;

        _isShooting = performed;
    }
}
