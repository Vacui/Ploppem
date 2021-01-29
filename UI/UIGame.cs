using TMPro;
using UnityEngine;

public class UIGame : MonoBehaviour {

    [SerializeField] TextMeshProUGUI _textScore = null;
    [SerializeField] TextMeshProUGUI _textHighScore = null;
    [SerializeField] TextMeshProUGUI _textTimer = null;

    private void Update() {
        if (_textTimer != null) {
            float gameSession = Time.time - GameManager.GameSessionStartTime;
            if (gameSession <= 0.0f) {
                gameSession = 0.0f;
            }
            string timer = "";
            System.TimeSpan t = System.TimeSpan.FromSeconds(gameSession);
            if (t.Hours > 0) {
                timer += $"{t.Hours.ToString("00")}:";
            }
            if (t.Minutes > 0) {
                timer += $"{t.Minutes.ToString("00")}:";
            }
            timer += t.Seconds.ToString("00");
            _textTimer.text = timer;
        }
    }

    public void UpdateScore(int value) {
        if (_textScore != null) {
            UpdateText(_textScore, value);
        }
    }

    public void UpdateHighScore() {
        if (_textHighScore != null) {
            UpdateText(_textHighScore, GameManager.HighScore);
        }
    }

    private void UpdateText(TextMeshProUGUI text, int value) {
        text.text = value.ToString();
    }

}