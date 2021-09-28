using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static TurnManager Instance;

    private void Start()
    {
        if (Instance != null)
        {
            Instance = this;
        }
    }
}
