using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetGridVisibleToggle : MonoBehaviour
{

    private Toggle _toggle;
    public void Start()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }


    private void OnToggleValueChanged(bool isOn)
    {
        UIEvents.FireGridVisibilityChanged(isOn);
    }
}
