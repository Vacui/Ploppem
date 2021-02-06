using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class responsible for manage vital game systems, like spawning, death of enemies, player score, etc...
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    private static float GAME_START_TIME = 0.5f;

    public static UnityEvent Paused = null;
    public static UnityEvent Resumed = null;
    static bool _isPaused = true;
    public static bool UsePauseEvents = true;
    public static bool IsPaused {
        get { return _isPaused; }
        set {
            _isPaused = value;

            if (UsePauseEvents) {
                if (IsPaused) {
                    Paused?.Invoke();
                    if (Instance != null) {
                        Instance.OnGamePause?.Invoke();
                    }
                } else {
                    Resumed?.Invoke();
                    if (Instance != null) {
                        Instance.OnGameResume?.Invoke();
                    }
                }
            }
        }
    }
    static bool _isGameOver = false;

    int _score = 0;
    int Score {
        get { return _score; }
        set {
            _score = value;
            if (_score > HighScore) {
                HighScore = _score;
            }
            OnGameScore?.Invoke(_score);
        }
    }

    public static UnityEvent NewHighScore = null;
    static int _highScore = 0;
    public static int HighScore {
        get { return _highScore; }
        private set {
            _highScore = value;
            DataManager.HighScore = value;
            NewHighScore?.Invoke();
        }
    }

    [Header("Stats")]
    [SerializeField] [ReadOnly] int _currentErrors = 0;
    public static int MAXERRORS = 0;
    [SerializeField] [Range(1, 9)] int _maxErrors = 5;
    int CurrentErrors {
        get { return _currentErrors; }
        set {
            _currentErrors = value;
            OnGameError?.Invoke(_currentErrors);
            if(_currentErrors  >= _maxErrors) {
                GameOver();
            }
        }
    }

    public static float GameSessionStartTime { get; private set; }


    [Header("Spawn")]
    [SerializeField] [ReadOnly] int _enemiesSpawned = 0;
    [SerializeField] [ReadOnly] int _enemiesCurrent = 0;
    [SerializeField] [ReadOnly] int _enemiesKilled = 0;
    [SerializeField] LayerMask _spawnLayerMask = 0;
    [SerializeField] [Range(0.5f, 2.0f)] float _spawnCheckRadius = 1.0f;
    private Transform _enemiesParent = null;

    [Header("Difficulty")]
    [SerializeField] [Min(0)] float _startingSpawnTime = 0.0f;
    [SerializeField] AnimationCurve _spawnTime = null;
    [SerializeField] [ReadOnly] float _lastSpawnTime = 0.0f;
    private float _currentSpawnTime { get { return _spawnTime.Evaluate(_score); } }
    [SerializeField] AnimationCurve _moveSpeed = null;
    private float _currentMoveSpeed { get { return _moveSpeed.Evaluate(_score); } }
    [SerializeField] AnimationCurve _deathTime = null;
    private float _currentDeathTime { get { return _deathTime.Evaluate(_score); } }
    [SerializeField] AnimationCurve _enemiesQuantityLimit = null;
    private float _currentEnemiesQuantityLimit { get { return _enemiesQuantityLimit.Evaluate(_score); } }
    [SerializeField] GameObject _enemyPrefab = null;

    [Header("Events")]
    public UnityIntEvent OnGameScore = null;
    public UnityIntEvent OnGameError = null;
    [Header("Controls")]
    public UnityEvent OnGamePause = null;
    public UnityEvent OnGameResume = null;
    [Header("Game Status")]
    public UnityEvent OnGameReady = null;
    public UnityEvent OnGameStart = null;
    public UnityEvent OnGameOver = null;

    [Header("Debug")]
    [SerializeField] AnimationCurve _spawnStory = new AnimationCurve();


    private void OnEnable() {
        Enemy.Killed += Scored;
        Enemy.Depleted += Error;
        Enemy.Dead += ProgramNewSpawn;
    }

    private void OnDisable() {
        Enemy.Killed -= Scored;
        Enemy.Depleted -= Error;
        Enemy.Dead -= ProgramNewSpawn;
    }

    private void Awake() {
        Random.InitState(Random.Range(0, Mathf.RoundToInt(Mathf.Infinity)));
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        Instance = this;
    }

    private void GameInitialize() {
        HighScore = DataManager.HighScore;
        Score = 0;
        MAXERRORS = _maxErrors;
        CurrentErrors = 0;
        _lastSpawnTime = 0;
        UsePauseEvents = true;
        StopAllCoroutines();
        _enemiesCurrent = 0;
        _enemiesSpawned = 0;
        _enemiesKilled = 0;
        _isGameOver = false;
        GameSessionStartTime = Time.time;
    }

    private void KillEnemies(bool recreate) {
        if (_enemiesParent != null) {
            Destroy(_enemiesParent.gameObject);
        }
        if (recreate) {
            _enemiesParent = new GameObject("Enemies Parent").transform;
            _enemiesParent.SetParent(transform);
            _enemiesParent.position = Vector3.zero;
        }
    }

    public void GameReady() {
        GameInitialize();
        GameResume();
        Debug.Log("Game State A (Ready)");
        OnGameReady?.Invoke();
        TimerManager.Create(gameObject, GAME_START_TIME, () => { GameStart(); });
    }

    public void GameStart() {
        _spawnStory = new AnimationCurve();
        _spawnStory.AddKey(0, 0);

        Debug.Log("Game State B (Start)");

        // Reset variables
        GameInitialize();

        // Recreate Enemies Parent
        KillEnemies(true);

        OnGameStart?.Invoke();

        // Fill the screen with entities
        for (int i = 0; i < _currentEnemiesQuantityLimit; i++) {
            ProgramNewSpawn(_startingSpawnTime);
            _enemiesSpawned++;
        }
    }

    public void GamePause() {
        if (!IsPaused) {
            Debug.Log("Game Pause");
            IsPaused = true;
            Time.timeScale = 0;
        }
    }
    public void GameResume() {
        if (IsPaused) {
            Debug.Log("Game Resume");
            IsPaused = false;
            Time.timeScale = 1;
        }
    }

    private void ProgramNewSpawn() { ProgramNewSpawn(_currentSpawnTime); }
    private void ProgramNewSpawn(float spawnTime = 0.0f) {
        if(_enemiesCurrent > 0) {
            _enemiesCurrent--;
        }

        StartCoroutine(SpawnCooldown(spawnTime));
    }

    IEnumerator SpawnCooldown(float spawnTime) {
        _lastSpawnTime += spawnTime;
        _spawnStory.AddKey(Time.time, _lastSpawnTime);

        float t = 0;
        float limit = _lastSpawnTime;
        while (t < limit) {
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
        }

        _lastSpawnTime -= spawnTime;
        _spawnStory.AddKey(Time.time, _lastSpawnTime);

        Spawn();
    }

    public void Spawn() {
        bool success = true;
        if (!IsPaused && _enemiesCurrent < _currentEnemiesQuantityLimit) {
            try {
                Enemy newEnemy = Instantiate(_enemyPrefab, RandomEnemyPosition(), Quaternion.identity, _enemiesParent).GetComponent<Enemy>();
                if(newEnemy != null) {
                    newEnemy.SetUp(_currentDeathTime, _currentMoveSpeed);
                }
                _enemiesSpawned++;
                _enemiesCurrent++;
            } catch (System.Exception e) {
                Debug.LogWarning($"Error spawning enemy: {e.Message}", gameObject);
                success = false;
            }
        } else {
            success = false;
        }

        if (!success && _enemiesCurrent < _currentEnemiesQuantityLimit && !_isGameOver) {
            ProgramNewSpawn();
        }
    }

    private Vector2 RandomEnemyPosition() {
        int maxChecks = 10;
        int checks = 0;
        Vector2 enemyPos = Vector2.zero;
        bool posIsCorrect = false;
        while(!posIsCorrect && checks < maxChecks) {
            enemyPos = GameCamera.GetRandomPos();
            posIsCorrect = Physics2D.OverlapCircle(enemyPos, _spawnCheckRadius, _spawnLayerMask) == null;
            checks++;
        }
        if (checks >= maxChecks) {
            throw new System.Exception("No suitable positions");
        } else {
            return enemyPos;
        }
    }

    private void Scored() {
        Score += 1;
        _enemiesKilled++;
    }

    public void Error() {
        CurrentErrors += 1;
    }

    public void GameOver() {
        Debug.Log("Game State C (Over)");
        _isGameOver = true;
        StopAllCoroutines();
        GamePause();
        KillEnemies(false);
        GameSessionStartTime = 0.0f;
        OnGameOver?.Invoke();
    }

    public void GameQuit() {
        Application.Quit();
    }

    private void OnDrawGizmosSelected() {
        if (!Application.isPlaying) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Vector3.zero, _spawnCheckRadius);
        }
    }

}

[System.Serializable]
public class DifficultyElement {
    public GameObject Prefab = null;
    [Range(0, 100)] public int Chance = 0;
}
