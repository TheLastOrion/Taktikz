using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Interfaces;
using Interfaces;
using UnityEngine;

public class Node :  IHighlightable, ISelectable
{
    private GameObject _nodeOutline;
    private int _x;
    private int _y;
    private int _cellSize;
    private bool _isBlocked;
    private MeshRenderer _renderer;

    public Node(int x, int y, int cellSize, GameObject outline, Vector3 pos , Transform parent, bool isBlocked = false)
    {
        _isBlocked = isBlocked;
        _x = x;
        _y = y;
        _cellSize = cellSize;
        GameObject go = GameObject.Instantiate(outline, pos, Quaternion.identity);
        go.transform.localScale *= _cellSize;
        go.transform.parent = parent;
        StringBuilder name = new StringBuilder();
        
        //Give names to nodes for debug purposes
        name.AppendFormat("Node X:{0} Y{1}", x+1, y+1);
        go.name = name.ToString();
        AssignOutline(go);
    }
    public void SetVisible(bool active)
    {
        _nodeOutline.GetComponent<MeshRenderer>().enabled = active;
    }

    public void Highlight(bool active)
    {
    }

    public void AssignOutline(GameObject outline)
    {
        _nodeOutline = outline;
        _renderer = _nodeOutline.GetComponent<MeshRenderer>();
    }

    public ISelectable Select()
    {
        return this;
    }

    public void Deselect()
    {
        
    }
}
