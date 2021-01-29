using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyGradient {
    public Color color0 = Color.white;
    public Color color1 = Color.white;
    public Gradient gradient {
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

    public EnemyGradient(Color newColor0, Color newColor1) {
        color0 = newColor0;
        color1 = newColor1;
    }
}

public class EnemyGradientManager : MonoBehaviour {

    public static EnemyGradientManager Instance = null;

    [SerializeField] int _forcedGradient = -1;
    [SerializeField] List<EnemyGradient> _gradients = new List<EnemyGradient>();
    [SerializeField] [ReadOnly] int _currentIndex = 0;
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

        DataManager.EnemyGradient = 0;
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
        if (newIndex >= _gradients.Count) {
            newIndex = 0;
        } else {
            if (newIndex < 0) {
                newIndex = _gradients.Count - 1;
            }
        }
        _currentIndex = Mathf.Clamp(newIndex, 0, _gradients.Count - 1);

        EnemyGradient newGradient = _gradients[_currentIndex];
        if (_UIGradient != null) {
            _UIGradient.SetGradient(newGradient.gradient);
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

    public static Gradient GetCurrentGradient() {
        Gradient result = new Gradient();
        if(Instance != null) {
            result = Instance._gradients[Instance._currentIndex].gradient;
        }
        return result;
    }

}