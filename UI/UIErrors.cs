using TMPro;
using UnityEngine;

/// <summary>
/// Class for game UI element showing player errors num.
/// </summary>
public class UIErrors : MonoBehaviour {

    [Header("Components")]
    [SerializeField] TextMeshProUGUI _textNum = null;

    public void UpdateUI(int num) {
        if (_textNum != null) {
            _textNum.text = $"{num.ToString()}/{GameManager.MAXERRORS}";
        }
    }

}
