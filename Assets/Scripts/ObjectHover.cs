using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHover : MonoBehaviour
{
    private MeshRenderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        // Change the color of the GameObject to red when the mouse is over GameObject
        _renderer.material = GameField.Instance._highlightNodeMaterial;
    }

    void OnMouseExit()
    {
        // Reset the color of the GameObject back to normal
        _renderer.material = GameField.Instance._standardNodeMaterial;
    }
}
