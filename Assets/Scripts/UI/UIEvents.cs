using System;


public class UIEvents
{
    public static event Action<bool> GridVisibilityChanged;
    public static event Action<bool> PathCostVisibilityChanged;
    public static event Action<bool> CoordsVisibilityChanged;
    public static event Action SpawnEnemiesButtonPressed;
    public static event Action SpawnAlliesButtonPressed;

    public static void FireGridVisibilityChanged(bool isOn)
    {
        if (GridVisibilityChanged != null)
        {
            GridVisibilityChanged(isOn);
        }
    }

    public static void FirePathCostVisibilityChanged(bool isOn)
    {
        if (PathCostVisibilityChanged != null)
        {
            PathCostVisibilityChanged(isOn);
        }
    }

    public static void FireCoordsVisibilityChanged(bool isOn)
    {
        if (CoordsVisibilityChanged != null)
        {
            CoordsVisibilityChanged(isOn);
        }
    }

    public static void FireSpawnEnemiesButtonPressed()
    {
        if (SpawnEnemiesButtonPressed != null)
        {
            SpawnEnemiesButtonPressed();
        }
    }

    public static void FireSpawnAlliesButtonPressed()
    {
        if (SpawnAlliesButtonPressed != null)
        {
            SpawnAlliesButtonPressed();
        }
    }


}
