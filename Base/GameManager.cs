using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    public static float START_GAME_TIME = 0.5f;

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

    public static float GameSessionStartTime = 0.0f;


    [Header("Spawn")]
    [SerializeField] [ReadOnly] int _monstersSpawned = 0;
    [SerializeField] [ReadOnly] int _monstersCurrent = 0;
    [SerializeField] [ReadOnly] int _monstersKilled = 0;
    [SerializeField] LayerMask _spawnLayerMask = 0;
    [SerializeField] [Range(0.5f, 2.0f)] float _spawnCheckRadius = 1.0f;
    private Transform _monstersParent = null;

    [Header("Difficulty")]
    [SerializeField] [Min(0)] float _startingSpawnTime = 0.0f;
    [SerializeField] AnimationCurve _spawnTime = null;
    [SerializeField] [ReadOnly] float _lastSpawnTime = 0.0f;
    float _currentSpawnTime { get { return _spawnTime.Evaluate(_score); } }
    [SerializeField] AnimationCurve _moveSpeed = null;
    float _currentMoveSpeed { get { return _moveSpeed.Evaluate(_score); } }
    [SerializeField] AnimationCurve _deathTime = null;
    float _currentDeathTime { get { return _deathTime.Evaluate(_score); } }
    [SerializeField] AnimationCurve _monstersLimit = null;
    float _currentMonsterLimit { get { return _monstersLimit.Evaluate(_score); } }
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
        _monstersCurrent = 0;
        _monstersSpawned = 0;
        _monstersKilled = 0;
        _isGameOver = false;
        GameSessionStartTime = 0.0f;
    }

    private void KillEnemies(bool recreate) {
        if (_monstersParent != null) {
            Destroy(_monstersParent.gameObject);
        }
        if (recreate) {
            _monstersParent = new GameObject("Monsters Parent").transform;
            _monstersParent.SetParent(transform);
            _monstersParent.position = Vector3.zero;
        }
    }

    public void GameReady() {
        GameInitialize();
        GameResume();
        Debug.Log("Game State A (Ready)");
        OnGameReady?.Invoke();
        TimerManager.Create(gameObject, START_GAME_TIME, () => { GameStart(); });
    }

    public void GameStart() {
        _spawnStory = new AnimationCurve();
        _spawnStory.AddKey(0, 0);

        Debug.Log("Game State B (Start)");

        // Reset variables
        GameInitialize();

        // Recreate Monsters Parent
        KillEnemies(true);

        OnGameStart?.Invoke();

        // Fill the screen with entities
        for (int i = 0; i < _currentMonsterLimit; i++) {
            ProgramNewSpawn(_startingSpawnTime);
            _monstersSpawned++;
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
        if(_monstersCurrent > 0) {
            _monstersCurrent--;
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
        if (!IsPaused && _monstersCurrent < _currentMonsterLimit) {
            try {
                Enemy newEnemy = Instantiate(_enemyPrefab, RandomMonsterPosition(), Quaternion.identity, _monstersParent).GetComponent<Enemy>();
                if(newEnemy != null) {
                    newEnemy.SetUp(_currentDeathTime, _currentMoveSpeed);
                }
                _monstersSpawned++;
                _monstersCurrent++;
            } catch (System.Exception e) {
                Debug.LogWarning($"Error spawning enemy: {e.Message}", gameObject);
                success = false;
            }
        } else {
            success = false;
        }

        if (!success && _monstersCurrent < _currentMonsterLimit && !_isGameOver) {
            ProgramNewSpawn();
        }
    }

    private Vector2 RandomMonsterPosition() {
        int maxChecks = 10;
        int checks = 0;
        Vector2 monsterPos = Vector2.zero;
        bool posIsCorrect = false;
        while(!posIsCorrect && checks < maxChecks) {
            monsterPos = new Vector2(UnityEngine.Random.Range(-GameCamera.HalfWorldWidth, GameCamera.HalfWorldWidth), UnityEngine.Random.Range(-GameCamera.HalfWorlHeight + GameCamera.LIMIT_BOTTOM, GameCamera.HalfWorlHeight - GameCamera.LIMIT_TOP));
            posIsCorrect = Physics2D.OverlapCircle(monsterPos, _spawnCheckRadius, _spawnLayerMask) == null;
            checks++;
        }
        if (checks >= maxChecks) {
            throw new System.Exception("No suitable positions");
        } else {
            return monsterPos;
        }
    }

    private void Scored() {
        Score += 1;
        _monstersKilled++;
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