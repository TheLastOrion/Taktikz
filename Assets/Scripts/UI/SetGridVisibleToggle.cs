using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetGridVisibleToggle : MonoBehaviour
{

    private Toggle _gridVisibleToggle;
    public void Start()
    {
        _gridVisibleToggle = GetComponent<Toggle>();
        _gridVisibleToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }


    private void OnToggleValueChanged(bool isOn)
    {
        UIEvents.FireGridVisibilityChanged(isOn);
    }
}
