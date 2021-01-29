using UnityEngine;

public static class DataManager {

    public static float Tick = 0.01f;

    private const string HIGHSCORE_PLAYERPREFS_KEY = "highScore"; 
    public static int HighScore {
        get {
            return PlayerPrefs.GetInt(HIGHSCORE_PLAYERPREFS_KEY);
        }
        set {
            PlayerPrefs.SetInt(HIGHSCORE_PLAYERPREFS_KEY, value < 0 ? 0 : value);
        }
    }

    private const string PALETTE_PLAYERPREFS_KEY = "palette";
    public static bool Palette {
        get {
            return PlayerPrefs.GetInt(PALETTE_PLAYERPREFS_KEY) == 0;
        }
        set {
            PlayerPrefs.SetInt(PALETTE_PLAYERPREFS_KEY, value ? 0 : 1);
        }
    }

    private const string BACKGROUND_PLAYERPREFS_KEY = "background";
    public static int Background {
        get {
            return PlayerPrefs.GetInt(BACKGROUND_PLAYERPREFS_KEY);
        }
        set {
            PlayerPrefs.SetInt(BACKGROUND_PLAYERPREFS_KEY, value < 0 ? 0 : value);
        }
    }

    private const string ENEMYGRADIENT_PLAYERPREFS_KEY = "enemyGradient";
    public static int EnemyGradient {
        get {
            return PlayerPrefs.GetInt(ENEMYGRADIENT_PLAYERPREFS_KEY);
        }
        set {
            PlayerPrefs.SetInt(ENEMYGRADIENT_PLAYERPREFS_KEY, value < 0 ? 0 : value);
        }
    }

    private const string LANGUAGE_PLAYERPREFS_KEY = "language";
    public static string Language {
        get {
            return PlayerPrefs.GetString(LANGUAGE_PLAYERPREFS_KEY);
        }
        set {
            PlayerPrefs.SetString(LANGUAGE_PLAYERPREFS_KEY, value);
        }
    }

    #region Game stats

    private const string PLAYTIME_PLAYERPREFS_KEY = "playtime";
    public static float Playtime {
        get {
            return PlayerPrefs.GetFloat(PLAYTIME_PLAYERPREFS_KEY);
        }
        set {
            PlayerPrefs.SetFloat(PLAYTIME_PLAYERPREFS_KEY, value < 0 ? 0 : value);
        }
    }

    private const string HITS_PLAYERPREFS_KEY = "hits";
    public static int Hits {
        get {
            return PlayerPrefs.GetInt(HITS_PLAYERPREFS_KEY);
        }
        set {
            PlayerPrefs.SetInt(HITS_PLAYERPREFS_KEY, value < 0 ? 0 : value);
        }
    }

    private const string MISS_PLAYERPREFS_KEY = "miss";
    public static int Miss {
        get {
            return PlayerPrefs.GetInt(MISS_PLAYERPREFS_KEY);
        }
        set {
            PlayerPrefs.SetInt(MISS_PLAYERPREFS_KEY, value < 0 ? 0 : value);
        }
    }

    public static int TotalTouches {
        get {
            return Miss + Hits;
        }
    }

    public static float Precision {
        get {
            return Hits / (float)TotalTouches;
        }
    }

    #endregion

}