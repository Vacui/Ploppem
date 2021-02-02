using System;
using System.Collections.Generic;
using UnityEngine;

public enum LangElement {
    NULL,
    PLAY,
    HELP,
    CREDITS,
    CUSTOMIZATIONS,
    BACKGROUNDS,
    ENEMIES,
    VERSION
}

/// <summary>
/// Class responsible for managing the change of UI language
/// </summary>
public class LangManager : MonoBehaviour {

    /// <summary>
    /// Text Fields
    /// Useage: Fields[key]
    /// Example: Lang.Fields["world"]
    /// </summary>
    private static Dictionary<string, string> Fields = new Dictionary<string, string>();

    private void Awake() {
        LoadLanguage();
    }

    public void LoadLanguage(string lang) {
        if (lang == "") {
            lang = "eng";
        }
        DataManager.Language = lang;
        LoadLanguage();
    }

    /// <summary>
    /// Load language files from resources.
    /// </summary>
    /// Code source unknown.
    private static void LoadLanguage() {
        if (Fields == null) {
            Fields = new Dictionary<string, string>();
        }
        Fields.Clear();

        TextAsset textAsset = Resources.Load(@"Lang/" + DataManager.Language) as TextAsset;
        if (textAsset == null) {
            Debug.LogWarning($"The language -{DataManager.Language}- does not have a related file");
            textAsset = Resources.Load(@"Lang/eng") as TextAsset;
        }

        string[] lines = textAsset.text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        string key, value;
        for (int i = 0; i < lines.Length; i++) {
            int indexSep = lines[i].IndexOf("=");
            if (indexSep >= 0 && !lines[i].StartsWith("#")) {
                key = lines[i].Substring(0, indexSep);
                value = lines[i].Substring(
                    lines[i].IndexOf("=") + 1,
                    lines[i].Length - indexSep - 1)
                    .Replace("\\n", Environment.NewLine);
                Fields.Add(key, value);
            }
        }
        Fields.Add("VERSION_NUM", Application.version);

        UpdateTexts();
    }

    private static void UpdateTexts() {
        foreach (UITextElement txtElement in FindObjectsOfType<UITextElement>()) {
            txtElement.SetUp();
        }
    }

    public static string GetText(string key) {
        if (Fields == null) {
            LoadLanguage();
            return GetText(key);
        } else {
            if (Fields.ContainsKey(key)) {
                return Fields[key];
            } else {
                return "";
            }
        }

    }

}
