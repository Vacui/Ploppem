using TMPro;
using UnityEngine;

public class UIGameStats : MonoBehaviour {

    [SerializeField] TextMeshProUGUI _textPlaytime = null;
    [SerializeField] TextMeshProUGUI _textTotalTouches = null;
    [SerializeField] TextMeshProUGUI _textMiss = null;
    [SerializeField] TextMeshProUGUI _textHits = null;
    [SerializeField] TextMeshProUGUI _textPrecision = null;

    private void OnEnable() {
        UpdateGameStats();
    }

    public void UpdateGameStats() {
        if (_textPlaytime != null) {
            System.TimeSpan t = System.TimeSpan.FromSeconds(DataManager.Playtime);
            _textPlaytime.text = $"\n{t.Days}\n{t.Hours.ToString("00")}\n{t.Minutes.ToString("00")}\n{t.Seconds.ToString("00")}";
        }
        if (_textTotalTouches != null) {
            _textTotalTouches.text = $"{DataManager.TotalTouches}";
        }
        if (_textMiss != null) {
            _textMiss.text = $"{DataManager.Miss}";
        }
        if (_textHits != null) {
            _textHits.text = $"{DataManager.Hits}";
        }
        if (_textPrecision != null) {
            _textPrecision.text = $"{DataManager.Precision * 100}%";
        }
    }

}