using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Cooldown in seconds.
/// </summary>
public class Cooldown : MonoBehaviour {

    [Header("Cooldown parameters")]
    [SerializeField] [Range(0.0f, 5.0f)]
    private float _duration = 0.0f;
    
    [SerializeField] [ReadOnly]
    private float _currentTime = 0.0f;
    private float CurrentTime {
        get {
            return _currentTime;
        }
        set {
            _currentTime = Mathf.Clamp(value, 0.0f, _duration);
        }
    }

    private float Percentage {
        get {
            float result = 1;
            if (_duration > 0) {
                result = _currentTime / _duration;
            }
            return Mathf.Clamp(result, 0.0f, 1.0f);
        }
    }
    [SerializeField] [ReadOnly] bool _isStopped = false;

    [SerializeField] UnityFloatEvent ChangedPercentage = null;
    [SerializeField] UnityEvent Ended = null;
    [SerializeField] UnityEvent Stopped = null;

    private void OnValidate() {
        if (!Application.isPlaying) {
            SetCurrentTime(_duration);
        }
    }

    private void Update() {
        if (!GameManager.IsPaused && !_isStopped && _currentTime < _duration) {
            SetCurrentTime(_currentTime + Time.deltaTime);
        }
    }

    public void CooldownSetupAndStart(float duration) {
        _isStopped = true;
        _duration = duration;
        CooldownStart();
    }

    public void CooldownStart() {
        if (_duration > 0) {
            SetCurrentTime(0);
            _isStopped = false;
        }
    }

    public virtual void SetCurrentTime(float value) {
        _currentTime = value;
        ChangedPercentage?.Invoke(Percentage);

        if (Application.isPlaying == false) {
            EnemyRadialSlider rs = GetComponent<EnemyRadialSlider>();
            if(rs != null) {
                rs.SetPercentage(1);
            }
        }
        if (Application.isPlaying && _currentTime >= _duration) {
            CooldownStop();
            CooldownEnd();
        }
    }

    public void CooldownStop() {
        Debug.Log("Cooldown Stop");
        _isStopped = true;
        Stopped?.Invoke();
    }

    public virtual void CooldownEnd() {
        Debug.Log("Cooldown End");
        Ended?.Invoke();
    }

}
