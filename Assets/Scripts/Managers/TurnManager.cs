using System.Collections;
using System.Collections.Generic;
using Enumerations;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static TurnManager Instance;
    [SerializeField] private TextMeshProUGUI _turnIndicatorText;
    private TurnType _turnType;
    public TurnType TurnType
    {
        get { return _turnType; }
        set { _turnType = value; }
    }
    private void Start()
    {
        GameEvents.TurnChanged += GameEvents_TurnChanged;
        if (Instance != null)
        {
            Instance = this;
        }
        GameEvents.FireTurnChanged(TurnType.PlayerTurn);

    }

    private void GameEvents_TurnChanged(TurnType newTurn)
    {
        if (newTurn == TurnType.PlayerTurn)
        {
            _turnIndicatorText.color = Color.cyan;
            _turnIndicatorText.text = "PLAYER TURN";
        }
        else if (newTurn == TurnType.AITurn)
        {
            _turnIndicatorText.color = Color.red;
            _turnIndicatorText.text = "AI TURN";
        }
    }
}
