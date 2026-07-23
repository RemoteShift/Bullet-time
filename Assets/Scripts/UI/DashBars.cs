using UnityEngine;
using UnityEngine.UI;

public class DashBars : MonoBehaviour
{
    [SerializeField] private RawImage[] _dashBars;
    
    private void Start()
    {
        RefreshBars();
    }

    private void OnEnable()
    {
        LocomotionController.Instance.OnDashCountChanged.AddListener(RefreshBars);
    }

    private void OnDisable()
    {
        LocomotionController.Instance.OnDashCountChanged.RemoveListener(RefreshBars);
    }

    private void RefreshBars()
    {
        int activeDashCount = Mathf.Clamp(LocomotionController.Instance.DashCount, 0, _dashBars.Length);

        for (int i = 0; i < _dashBars.Length; i++)
        {
            _dashBars[i].enabled = i < activeDashCount;
        }
    }
}
