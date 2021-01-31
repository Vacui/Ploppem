using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible of manage the enemy visual radial slider, that shows its death countdown.
/// </summary>
public class EnemyRadialSlider : MonoBehaviour {

    [Header("Radial Parameters")]
    [SerializeField] [Range(0.0f, 1.0f)] float _currentPercentage = 0.0f;
    private float CurrentPercentage {
        get { return _currentPercentage; }
        set {
            _currentPercentage = Mathf.Clamp(value, 0.0f, 1.0f);
        }
    }

    [Header("Extra")]
    [SerializeField] MyGradient _color = null;
    [SerializeField] List<SpriteRenderer> _otherSr = new List<SpriteRenderer>();
    [SerializeField] TrailRenderer _myTrailRenderer = null;

    [Header("Components")]
    [SerializeField] SpriteRenderer _leftHalf = null;
    [SerializeField] SpriteMask _leftHalfMask = null;
    [SerializeField] SpriteRenderer _rightHalf = null;
    [SerializeField] SpriteMask _rightHalfMask = null;

    private void OnValidate() {
        if (!Application.isPlaying) {
            SetPercentage(_currentPercentage);
        }
    }

    private void Awake() {
        if (_currentPercentage <= 0) {
            SetPercentage(0);
        }
        SetGradient(EnemyGradientManager.Instance.CurrentGradient);
    }

    public void SetPercentage(float value) {

        CurrentPercentage = value;

        _leftHalfMask.transform.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp((180 * _currentPercentage) * 2, 0, 180));
        _rightHalfMask.transform.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(((180 * (_currentPercentage - 0.5f)) * 2) - 180, -180, 0));

        UpdateColor();
    }

    public void SetGradient(Gradient newGradient) {
        _color._gradient = newGradient;
        UpdateColor();
    }

    public void UpdateColor() {
        Color color = _color._gradient.Evaluate(_currentPercentage);

        if (_otherSr != null && _otherSr.Count > 0) {
            foreach (SpriteRenderer _sr in _otherSr) {
                if (_sr != null) {
                    _sr.color = color;
                }
            }
        }

        if (_leftHalf != null) { _leftHalf.color = color; }
        if (_rightHalf != null) { _rightHalf.color = color; }

        if (_myTrailRenderer != null) {
            _myTrailRenderer.colorGradient = GradientUtils.GenerateGradient(color, color, 1, 1);
        }
    }

    public void Hide() {
        if (_leftHalf != null) { _leftHalf.gameObject.SetActive(false); }
        if (_rightHalf != null) { _rightHalf.gameObject.SetActive(false); }
        if(_myTrailRenderer != null) { _myTrailRenderer.enabled = false; }
    }

}
