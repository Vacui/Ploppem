using UnityEngine.UI;
using UnityEngine;

public class ForceAction : MonoBehaviour {

    public void OnClick() {
        Button myButton = GetComponent<Button>();
        if (myButton != null) {
            myButton.onClick?.Invoke();
        }
    }

    public void Destroy() {
        Destroy(gameObject);
    }

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
