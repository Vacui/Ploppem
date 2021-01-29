using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour {

    [SerializeField] List<Sprite> _backgrounds = new List<Sprite>();
    [SerializeField] [ReadOnly] int _currentIndex = 0;
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
        if (newIndex >= _backgrounds.Count) {
            newIndex = 0;
        } else {
            if (newIndex < 0) {
                newIndex = _backgrounds.Count - 1;
            }
        }
        _currentIndex = Mathf.Clamp(newIndex, 0, _backgrounds.Count - 1);

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