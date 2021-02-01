using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible to manage the change and store the Background.
/// </summary>
public class BackgroundManager : MonoBehaviour {

    [SerializeField] List<Sprite> _backgrounds = new List<Sprite>();
    [SerializeField] [ReadOnly] int _currentIndex = 0;
    int CurrentIndex {
        get { return _currentIndex; }
        set {
            if (_backgrounds == null) {
                Debug.LogWarning($"The BackgroundManager {name} has no backgrounds", gameObject);
                _currentIndex = 0;
            } else {
                if (value >= _backgrounds.Count) {
                    value = 0;
                } else if (value < 0) {
                    value = _backgrounds.Count - 1;
                }
                _currentIndex = Mathf.Clamp(value, 0, _backgrounds.Count - 1);
            }
        }
    }

    [SerializeField] SpriteRenderer _spriteRenderer = null;
    [SerializeField] Image _image = null;

    private void Awake() {
        DataManager.Background = 0;
        UpdateShape(DataManager.Background);
    }

    public void NextShape() {
        UpdateShape(_currentIndex + 1);
    }

    public void PrevShape() {
        UpdateShape(_currentIndex - 1);
    }

    public void UpdateShape() {
        UpdateShape(DataManager.Background);
    }
    public void UpdateShape(int newIndex) {
        _currentIndex = newIndex;
        Sprite newBackground = _backgrounds[_currentIndex];
        if (_spriteRenderer != null) {
            _spriteRenderer.sprite = newBackground;
        }
        if (_image != null) {
            _image.sprite = newBackground;
        }

        DataManager.Background = _currentIndex;
    }

}
