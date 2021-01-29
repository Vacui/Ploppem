using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour {

    Camera _myCamera = null;
    public static float LIMIT_TOP = 1.8f;
    public static float LIMIT_BOTTOM = 1.8f;

    public static float Height {
        get;
        private set;
    }
    public static float Width {
        get;
        private set;
    }
    public static Vector2 ScreenBounds {
        get;
        private set;
    }
    public static float WorldHeight {
        get;
        private set;
    }
    public static float HalfWorlHeight {
        get;
        private set;
    }
    public static float WorldWidth {
        get;
        private set;
    }
    public static float HalfWorldWidth {
        get;
        private set;
    }

    private void Awake() {
        _myCamera = GetComponent<Camera>();
        Height = Screen.height;
        Width = Screen.width;
        ScreenBounds = _myCamera.ScreenToWorldPoint(new Vector3(Width, Height, transform.position.z));
        Vector3 topRightCorner = _myCamera.ViewportToWorldPoint(new Vector3(1, 1, _myCamera.nearClipPlane));
        HalfWorlHeight = topRightCorner.y;
        WorldHeight = HalfWorlHeight * 2.0f;
        HalfWorldWidth = topRightCorner.x;
        WorldWidth = HalfWorldWidth * 2.0f;
    }

}