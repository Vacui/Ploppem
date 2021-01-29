using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PaletteElement : MonoBehaviour {

    [SerializeField] PaletteColor _color = 0;
    [SerializeField] [Range(0, 255)] byte _alpha = 255;
    [SerializeField] bool _isForced = false;

    Color _myColor = Color.white;

    private void OnValidate() {
        SetUp();
    }

    private void OnEnable() {
        SetUp();
    }

    public void SetUp() {
        if (_color != PaletteColor.NULL && _color != 0) {

            Color32 color = PaletteManager.GetColor(_color, _isForced);
            color = new Color32(color.r, color.g, color.b, _alpha);

            Image myImg = GetComponent<Image>();
            if (myImg != null) {
                myImg.color = color;
            }

            RawImage myRawImg = GetComponent<RawImage>();
            if (myRawImg != null) {
                myRawImg.color = color;
            }

            TextMeshProUGUI myText = GetComponent<TextMeshProUGUI>();
            if (myText != null) {
                myText.color = color;
            }

            SpriteRenderer mySpriteRenderer = GetComponent<SpriteRenderer>();
            if (mySpriteRenderer != null) {
                mySpriteRenderer.color = color;
            }

            MyGradient[] myGradients = GetComponents<MyGradient>();
            foreach(MyGradient myGradient in myGradients) {
                if (myGradient != null) {
                    myGradient._gradient = Utility.GenerateGradient(color, color, _alpha, _alpha);
                }
            }

            MyColor[] myColors = GetComponents<MyColor>();
            foreach(MyColor myColor in myColors) {
                if (myColor != null) {
                    myColor._color = color;
                }
            }
        }
    }

}