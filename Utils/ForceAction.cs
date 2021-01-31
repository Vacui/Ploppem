using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Class used by UI elements to perform special actions, such as call a URL in the browser or modify a boolean value in an Animator component.
/// </summary>
public class ForceAction : MonoBehaviour {

    public void ForceBtnClick() {
        Button myButton = GetComponent<Button>();
        if (myButton != null) {
            myButton.onClick?.Invoke();
        }
    }

    public void DestroyObject() {
        Destroy(gameObject);
    }

    // The presence of the methods SetAnimatorBoolTrue and False is necessary to overcome the interface limitation in the objects inspector for UnityEvents.
    public void SetAnimatorBoolTrue(string parameterName) {
        SetAnimatorBool(parameterName, true);
    }

    public void SetAnimatorBoolFalse(string parameterName) {
        SetAnimatorBool(parameterName, false);
    }

    public void SetAnimatorBool(string parameterName, bool value) {
        GetComponent<Animator>().SetBool(parameterName, value);
    }

    public void GoToURL(string url) {
        Application.OpenURL(url);
    }

}
