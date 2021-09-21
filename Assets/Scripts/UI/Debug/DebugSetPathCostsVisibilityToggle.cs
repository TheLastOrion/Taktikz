using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugSetPathCostsVisibilityToggle : MonoBehaviour
{
    private Toggle _toggle;
    public void Start()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }


    private void OnToggleValueChanged(bool isOn)
    {
        UIEvents.FirePathCostVisibilityChanged(isOn);
    }
}
