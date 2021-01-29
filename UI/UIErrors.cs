using TMPro;
using UnityEngine;

public class UIErrors : MonoBehaviour {

    [Header("Components")]
    [SerializeField] TextMeshProUGUI _textNum = null;

    public void UpdateUI(int num) {
        if (_textNum != null) {
            _textNum.text = num.ToString();
            _textNum.text += "/" + GameManager.MAXERRORS;
        }
    }

}