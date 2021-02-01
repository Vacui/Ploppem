using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Class responsible of detecting hits (touches) on enemies.
/// </summary>
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
        bool enemyHitted = false;
        Enemy enemy = null;
        Collider2D hit = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(position), _touchRadius, _hitMask);
        if (hit != null) {
            enemy = hit.gameObject.GetComponent<Enemy>();
            if (enemy != null && !enemy.IsDead) {
                enemy.Hit();
                enemyHitted = true;
            }
        }

        if (enemyHitted) {
            EnemyHitted?.Invoke();
        } else {
            EnemyMissed?.Invoke();
        }
    }

}
