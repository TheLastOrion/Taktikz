using System.Collections;
using System.Collections.Generic;
using Enumerations;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static TurnManager Instance;

    private TurnType _turnType;
    public TurnType TurnType
    {
        get { return _turnType; }
        set { _turnType = value; }
    }
    private void Start()
    {
        if (Instance != null)
        {
            Instance = this;
            _turnType = TurnType.PlayerTurn;
        }
    }
}
