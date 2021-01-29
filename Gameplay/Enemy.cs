using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {

    Rigidbody2D _myRigidbody2D = null;
    Collider2D _myCollider2D = null;
    Transform _myTransform = null;

    [Header("Movement")]
    [SerializeField] Direction _allowedDirections = 0;
    List<Direction> _possibleDirections = null;
    Direction _currentDirection;
    public Direction CurrentDirection {
        get { return _currentDirection; }
        set {
            _currentDirection = value;
            _currentDirectionVector2 = value.GetVector2();

            if (_possibleDirections.Count > 1) {
                _possibleDirections.Remove(_currentDirection);
                _directionsHistory.Enqueue(_currentDirection);
                if (_directionsHistory.Count > _maxDirectionsHistoryLength) {
                    Direction d = _directionsHistory.Dequeue();
                    _possibleDirections.Add(d);
                }
            }

            RestartChangeDirection();
        }
    }
    protected Vector2 _currentDirectionVector2;
    [SerializeField] [Range(0.1f, 2.0f)] float _changeDirectionRate = 0.5f;
    Queue<Direction> _directionsHistory = new Queue<Direction>();
    [SerializeField] [Range(1, 3)] int _maxDirectionsHistoryLength = 1;

    Coroutine _changeDirection = null;
    const float MAX_SPEED = 10;
    [SerializeField] [Min(0)] float _speed = 0.0f;
    public float Speed {
        get { return _speed; }
        set { _speed = Mathf.Clamp(value, 0, MAX_SPEED); }
    }

    [SerializeField] [Range(0.1f, 1)] float _raycastCheckRate = 0.1f;
    [SerializeField] LayerMask _raycastCheckLayerMask = 0;
    [SerializeField] [Min(1.0f)] float _raycastDistance = 1.0f;
    Coroutine _directionCheck = null;
    [SerializeField] EnemyDirectionGraphic _directionGraphic = null;

    [Header("Death")]
    [SerializeField] Cooldown _myCooldown = null;
    [SerializeField] Animator _myAnimator = null;
    [SerializeField] UnityEvent OnDying = null;

    public static event System.Action Killed = null;
    public static event System.Action Depleted = null;
    public static event System.Action Dead = null;
    

    public bool IsDead {
        get;
        private set;
    }

    private void Awake() {
        _myRigidbody2D = GetComponent<Rigidbody2D>();
        _myCollider2D = GetComponent<Collider2D>();
        _myTransform = transform;
    }

    public void SetUp(float deathTime, float speed) {
        if(_myCooldown != null) {
            _myCooldown.CooldownSetupAndStart(deathTime);
        }
        if(_directionGraphic != null) {
            _directionGraphic.SetUp();
        }
        Speed = speed;
        MoveStart();
    }

    

    private void FixedUpdate() {
        if (!GameManager.IsPaused) {
            Move();
        } else {
            _myRigidbody2D.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        NewDirection();
    }

    #region movement

    private void MoveStart() {
        Direction[] matching = FlagDirectionsToArray(_allowedDirections);

        if (matching.Length > 0) {
            _possibleDirections = matching.ToList();
            if (_possibleDirections.Contains(Direction.NULL)) {
                _possibleDirections.Remove(Direction.NULL);
            }
            NewDirection();
            if (_raycastCheckRate > 0.0f) {
                _directionCheck = StartCoroutine(CurrentDirectionCheck());
            }
        }
    }

    private Direction[] FlagDirectionsToArray(Direction flagDirection) {
        return System.Enum.GetValues(typeof(Direction))
                           .Cast<Direction>()
                           .Where(c => (flagDirection & c) == c)
                           .ToArray();
    }

    private void RestartChangeDirection() {
        if (_changeDirectionRate > 0.0f) {
            if (_changeDirection != null) {
                StopCoroutine(_changeDirection);
            }
            _changeDirection = StartCoroutine(ChangeDirectionTimer());
        }
    }

    RaycastHit2D[] hits;
    private bool DirectionIsSafe(Direction direction) {
        hits = Physics2D.RaycastAll(_myTransform.position, direction.GetVector3(), _raycastDistance, _raycastCheckLayerMask);
        bool isSafe = true;
        for (int i = 0; i < hits.Length && isSafe; i++) {
            RaycastHit2D hit = hits[i];
            if (hit.collider != null) {
                if (hit.collider.gameObject != gameObject) {
                    isSafe = false;
                }
            }
        }
        return isSafe;
    }

    private void NewDirection() {
        CurrentDirection = CurrentDirection.NewDirection(_possibleDirections);
        _directionGraphic.UpdateGraphic(_currentDirection, _possibleDirections);
    }

    private void Move() {
        if (!IsDead) {
            _myRigidbody2D.velocity = _currentDirectionVector2 * _speed;
        }
    }

    public void MoveStop() {
        Speed = 0;
        _myRigidbody2D.velocity = Vector2.zero;
        _myRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    IEnumerator ChangeDirectionTimer() {
        yield return new WaitForSeconds(_changeDirectionRate);
        if (!GameManager.IsPaused) {
            NewDirection();
        }
    }

    IEnumerator CurrentDirectionCheck() {
        while (true) {
            yield return new WaitForSeconds(_raycastCheckRate);
            if (!GameManager.IsPaused) {
                if (!DirectionIsSafe(CurrentDirection) || !DirectionIsSafe(CurrentDirection.Next()) || !DirectionIsSafe(CurrentDirection.Prev())) {
                    NewDirection();
                }
            }
        }
    }

    #endregion


    #region death

    public virtual void Hit() {
        Killed?.Invoke();
        IsDead = true;
        Dying();
    }

    public void Dying() { Dying(true); }
    public void Dying(bool destroy) {

        if (!IsDead) {
            Depleted?.Invoke();
            if (_myAnimator != null) {
                _myAnimator.SetTrigger("time");
            }
        } else {
            if (_myAnimator != null) {
                _myAnimator.SetTrigger("kill");
            }
        }

        if (destroy) {
            IsDead = true;

            Dead?.Invoke();
            OnDying?.Invoke();
            StopAllCoroutines();
            MoveStop();
            Destroy(gameObject, 0.5f);
        }
    }

    #endregion

    private void OnDrawGizmos() {

        if (Application.isPlaying && !IsDead) {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)_currentDirectionVector2 * _raycastDistance);
            Gizmos.DrawSphere(transform.position + (Vector3)_currentDirectionVector2 * _raycastDistance, 0.1f);

            Gizmos.color = Color.red;
            foreach (Direction value in _directionsHistory) {
                if (value != CurrentDirection) {
                    Gizmos.DrawLine(transform.position, transform.position + value.GetVector3() * _raycastDistance);
                }
            }
        } else {
            if (!Application.isPlaying) {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + Vector3.up * _raycastDistance);
            }
        }

    }

}