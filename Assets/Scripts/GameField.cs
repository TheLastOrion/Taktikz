﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameField : MonoBehaviour
{
    private static MyGrid _grid;
    [SerializeField]private GameObject _nodePrefab;
    [SerializeField]private int _width;
    [SerializeField]private int _height;
    [SerializeField]private int _cellSize;
    [SerializeField]private Transform _fieldTransform;
    [SerializeField]private Transform _nodesTransform;

    // Start is called before the first frame update
    void Start()
    {
        _grid = new MyGrid(_width, _height, _cellSize, transform.position, _nodePrefab, _nodesTransform);
        _fieldTransform.localScale = new Vector3(_width, 1, _height);
        UIEvents.GridVisibilityChanged += UIEvents_GridVisibilityChanged;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            Debug.LogFormat("Camera To World Point: {0}",Camera.main.ScreenToWorldPoint(Input.mousePosition));

        }
    }

    private void UIEvents_GridVisibilityChanged(bool isVisible)
    {
        SetNodeVisibility(isVisible);
    }

    // Update is called once per frame

    public void SetNodeVisibility(bool isVisible)
    {
        Node[,] nodes = _grid.GetNodes();
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                nodes[i,y].SetVisible(isVisible);
            }
        }
    }


}
