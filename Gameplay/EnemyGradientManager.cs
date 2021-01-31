using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for enemy palettes.
/// </summary>
[System.Serializable]
public class EnemyGradient {
    public Color color0 = Color.white;
    public Color color1 = Color.white;
    public Gradient Gradient {
        get {
            Gradient result = new Gradient();
            result.colorKeys = new GradientColorKey[2] {
                new GradientColorKey(color0, 0),
                new GradientColorKey(color1, 1)};
            result.alphaKeys = new GradientAlphaKey[2] {
                new GradientAlphaKey(255,0),
                new GradientAlphaKey(255,1)};
            return result;
        }
    }
}

/// <summary>
/// Class responsible to manage the change and store the enemy palette.
/// </summary>
public class EnemyGradientManager : MonoBehaviour {

    public static EnemyGradientManager Instance = null;

    [SerializeField] int _forcedGradient = -1;
    [SerializeField] List<EnemyGradient> _gradients = new List<EnemyGradient>();
    [SerializeField] [ReadOnly] int _currentIndex = 0;
    private int CurrentIndex {
        get { return _currentIndex; }
        set {
            if(_gradients == null) {
                Debug.LogWarning($"The EnemyGradientManager {name} has no gradients", gameObject);
                _currentIndex = 0;
            } else {
                if (value >= _gradients.Count) {
                    value = 0;
                } else {
                    if (value < 0) {
                        value = _gradients.Count - 1;
                    }
                }
                _currentIndex = Mathf.Clamp(value, 0, _gradients.Count - 1);
            }
        }
    }

    public Gradient CurrentGradient {
        get {
            return _gradients[_currentIndex].Gradient;
        }
    }
    [SerializeField] UIGradient _UIGradient = null;
    [SerializeField] Image _color0Img = null;
    [SerializeField] TextMeshProUGUI _color0Text = null;
    [SerializeField] Image _color1Img = null;
    [SerializeField] TextMeshProUGUI _color1Text = null;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        Instance = this;

        UpdateGradient(DataManager.EnemyGradient);
    }

    public void NextGradient() {
        UpdateGradient(_currentIndex + 1);
    }

    public void PrevGradient() {
        UpdateGradient(_currentIndex - 1);
    }

    public void UpdateGradient() {
        UpdateGradient(DataManager.EnemyGradient);
    }
    private void UpdateGradient(int newIndex) {
        if (_forcedGradient >= 0) {
            newIndex = _forcedGradient;
        }
        _currentIndex = newIndex;

        EnemyGradient newGradient = _gradients[_currentIndex];
        if (_UIGradient != null) {
            _UIGradient.Gradient = newGradient.Gradient;
        }
        if (_color0Img != null) {
            _color0Img.color = newGradient.color0;
        }
        if (_color0Text != null) {
            _color0Text.text = $"#{ColorUtility.ToHtmlStringRGB(newGradient.color0)}";
        }

        if (_color1Img != null) {
            _color1Img.color = newGradient.color1;
        }
        if (_color1Text != null) {
            _color1Text.text = $"#{ColorUtility.ToHtmlStringRGB(newGradient.color1)}";
        }

        DataManager.EnemyGradient = _currentIndex;
    }

}
