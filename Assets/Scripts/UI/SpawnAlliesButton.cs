
using UnityEngine;
using UnityEngine.UI;

public class SpawnAlliesButton : MonoBehaviour
{
    private Button _button;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        UIEvents.FireSpawnAlliesButtonPressed();
    }
}
