using UnityEngine;

public enum PaletteColor {
    NULL = 0,
    BLACK = 1,
    WHITE = 2,
    GRAY = 3
}

/// <summary>
/// Class responsible to manage the change and store the UI palette.
/// </summary>
public class PaletteManager : MonoBehaviour {

    static Color32 BLACK = new Color32(4, 4, 4, 255);
    static Color32 WHITE = new Color32(251, 251, 251, 255);
    static Color32 GRAY = new Color32(151, 151, 151, 255);

    static bool IS_BLACK = true;

    public static Color32 GetColor(PaletteColor value, bool isForced) {
        if (value != PaletteColor.NULL && value != 0) {

            //the value is valid
            if (value == PaletteColor.BLACK) {
                return isForced || IS_BLACK ? BLACK : WHITE;
            } else {
                if (value == PaletteColor.WHITE) {
                    return isForced || IS_BLACK ? WHITE : BLACK;
                } else {
                    return GRAY;
                }
            }
        } else {
            return Color.black;
        }
    }

    private void Awake() {
        SetBlack(DataManager.Palette);
    }

    public void SetBlack(bool value) {
        IS_BLACK = value;
        DataManager.Palette = value;
        UpdatePalettes();
    }

    private void UpdatePalettes() {
        foreach(PaletteElement pElement in FindObjectsOfType<PaletteElement>()) {
            pElement.SetUp();
        }
    }

}
