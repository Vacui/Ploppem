using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Limiter : MonoBehaviour {

    Camera _myCamera = null;

    [SerializeField] [Min(0)] float _depth = 1.0f;

    private void Awake() {
        _myCamera = GetComponent<Camera>();
    }

    private void Start() {
        CreateLimits();
    }

    private void CreateLimits() {
        NewLimit("top", transform, 0, GameCamera.WorldHeight / 2.0f - GameCamera.LIMIT_TOP, new Vector2(GameCamera.WorldWidth, _depth));
        NewLimit("bottom", transform, 0, -GameCamera.WorldHeight / 2.0f + GameCamera.LIMIT_BOTTOM, new Vector2(GameCamera.WorldWidth, _depth));
        NewLimit("right", transform, GameCamera.WorldWidth / 2.0f, 0, new Vector2(_depth, GameCamera.WorldHeight));
        NewLimit("left", transform, -GameCamera.WorldWidth / 2.0f, 0, new Vector2(_depth, GameCamera.WorldHeight));
    }

    private void NewLimit(string name, Transform parent, float posX, float posY, Vector2 lScale) {
        Transform newLimit = new GameObject(name).transform;
        newLimit.parent = parent;
        newLimit.position = new Vector2(posX, posY);
        newLimit.localScale = lScale;
        newLimit.gameObject.AddComponent<BoxCollider2D>();
        newLimit.gameObject.layer = LayerMask.NameToLayer("Limit");
    }

}