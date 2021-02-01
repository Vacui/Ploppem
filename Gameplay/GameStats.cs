using UnityEngine;

/// <summary>
/// Class responsible of keep track of player stats.
/// </summary>
public class GameStats : MonoBehaviour {

    private float _startTime = 0.0f;

    public void IncreaseMiss() {
        DataManager.Misses++;
    }

    public void IncreaseHits() {
        DataManager.Hits++;
    }

    public void StartPlaytime() {
        if (_startTime <= 0) {
            Debug.Log("Start game session");
            _startTime = Time.time;
        }
    }

    public void StopPlaytime() {
        if (_startTime > 0.0f) {
            Debug.Log("Stop game session");
            DataManager.Playtime += Time.time - _startTime;
            _startTime = 0.0f;
        }
    }

    public void ResetGameStats() {
        DataManager.Misses = 0;
        DataManager.Hits = 0;
        DataManager.Playtime = 0;
    }

    private void OnApplicationQuit() {
        StopPlaytime();
    }

}
