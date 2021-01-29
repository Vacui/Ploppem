using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HitDetector : MonoBehaviour {

    [SerializeField] [Range(0.0f, 1.0f)] float _touchRadius = 0.0f;
    [SerializeField] LayerMask _hitMask = 0;

    [SerializeField] UnityEvent EnemyMissed = null;
    [SerializeField] UnityEvent EnemyHitted = null;

    private void Update() {

        if (!GameManager.IsPaused && !EventSystem.current.IsPointerOverGameObject()) {
            if (Input.touches.Length >= 1 && Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Began) {
                NewInput(Input.GetTouch(Input.touchCount - 1).position);
            } else {
                if (Input.GetMouseButtonDown(0)) {
                    NewInput(Input.mousePosition);
                }
            }
        }
    }

    private void NewInput(Vector2 position) {
        bool monsterHitted = false;
        Enemy monster = null;
        Collider2D hit = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(position), _touchRadius, _hitMask);
        if (hit != null) {
            monster = hit.gameObject.GetComponent<Enemy>();
            if (monster != null && !monster.IsDead) {
                monster.Hit();
                monsterHitted = true;
            }
        }

        if (monsterHitted) {
            EnemyHitted?.Invoke();
        } else {
            EnemyMissed?.Invoke();
        }
    }
}