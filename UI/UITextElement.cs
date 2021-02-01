using TMPro;
using UnityEngine;

/// <summary>
/// Class utilized to manage language change in UI Texts.
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class UITextElement : MonoBehaviour {

    TextMeshProUGUI _myTMPRO = null;
    [SerializeField] [TextArea] string text = "";

    private void OnValidate() {
        if (!Application.isPlaying && text.Length > 0) {
            GetComponent<TextMeshProUGUI>().text = text;
        }
    }

    private void OnEnable() {
        SetUp();
    }

    public void SetUp() {
        if (_myTMPRO == null) {
            _myTMPRO = GetComponent<TextMeshProUGUI>();
        }

        _myTMPRO.text = ReplacePlaceholders(text);
    }

    private string ReplacePlaceholders(string text) {
        if (text.Length > 0) {
            if (text.Contains("{") && text.Contains("}")) {
                int start = text.IndexOf('{') + 1;
                int end = text.IndexOf('}', start);
                string placeHolder = text.Substring(start, end - start);
                return text.Substring(0, start - 1) + LangManager.GetText(placeHolder) + ReplacePlaceholders(text.Substring(end + 1));
            }
        }
        return text;
    }

}
