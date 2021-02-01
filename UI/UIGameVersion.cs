using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class responsible of updating the version text in the Main Menu UI.
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class UIGameVersion : MonoBehaviour {

    [SerializeField] string _prefix = string.Empty;
    [SerializeField] string _suffix = string.Empty;

    [SerializeField] UnityEvent OnUpdate = null;

    private void Start() {
        GetComponent<TextMeshProUGUI>().text += $"{_prefix}{Application.version}{_suffix}";
        OnUpdate?.Invoke();
    }

}
