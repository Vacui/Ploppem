using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Master class for UI panels, it manages the show and hide status.
/// </summary>
public class UIElement : MonoBehaviour {

    [SerializeField] GameObject _content = null;
    [SerializeField] bool _showOnAwake = false;

    public UnityEvent OnShowUI = null;
    public UnityEvent OnHideUI = null;

    private void Awake() {
        if (_showOnAwake) {
            ShowUI();
        } else {
            HideUI();
        }
    }

    private bool HasContent() {
        bool result = _content != null;
        if (result == false) {
            Debug.LogWarning($"the UIElement {name} has no content", gameObject);
        }
        return result;
    }

    public void ToggleUI() {
        if (HasContent()) {
            if (_content.activeSelf) {
                HideUI();
            } else {
                ShowUI();
            }
        }
    }

    public void ShowUI() {
        if (HasContent()) {
            _content.SetActive(true);
        }
        OnShowUI?.Invoke();
    }

    public void HideUI() {
        if (HasContent()) {
            _content.SetActive(false);
        }
        OnHideUI?.Invoke();
    }

}
