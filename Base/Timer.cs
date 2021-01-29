using UnityEngine;
using UnityEngine.Events;

public static class TimerManager {

    public static void Create(GameObject obj, float duration, System.Action onComplete) {
        Timer newTimer = obj.AddComponent<Timer>();
        newTimer.SetUp(duration);
        newTimer.OnCompleteAction += onComplete;
    }

}

public class Timer : MonoBehaviour {

    [SerializeField] [Min(0)] float _duration = 0;
    public UnityEvent OnComplete = null;
    public System.Action OnCompleteAction = null;

    float _time = 0;
    bool _completed = false;
    static float _destroyTime = 1.0f;

    private void Awake() {
        if (Utility.IsPositive(_duration)) {
            SetUp(_duration);
        }
    }

    public void SetUp(float duration) {
        if(duration < 0) {
            duration = 0;
        }
        _time = duration;
        _completed = false;
    }

    private void Update() {
        if (_time > 0 && !_completed) {
            _time -= Time.deltaTime;
            if (_time <= 0) {
                _completed = true;
                OnComplete?.Invoke();
                OnCompleteAction?.Invoke();
                Destroy(this, _destroyTime);
            }
        }
    }
}