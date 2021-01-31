/*
 * Customized version of UIGradient class made by azixMcAze.
 * Original: https://github.com/azixMcAze/Unity-UIGradient
 * 
 * I adapted its structure to support my Gradient class instead of 2 colors.
 * */
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to manage the UI gradient element in the customization menu for enemies palette.
/// </summary>
public class UIGradient : BaseMeshEffect
{
    [SerializeField] Gradient _gradient = new Gradient();
    public Gradient Gradient {
        get { return _gradient; }
        set {
            _gradient = value;
            GetComponent<Image>().enabled = false;
            GetComponent<Image>().enabled = true;
        }
    }
    [SerializeField] [Range(-180f, 180f)] float m_angle = 0f;
    [SerializeField] bool m_ignoreRatio = true;
    VertexHelper _myVh = null;

    public override void ModifyMesh(VertexHelper vh) {
        if (enabled) {
            _myVh = vh;
            Rect rect = graphic.rectTransform.rect;
            Vector2 dir = UIGradientUtils.RotationDir(m_angle);

            if (!m_ignoreRatio)
                dir = UIGradientUtils.CompensateAspectRatio(rect, dir);

            UIGradientUtils.Matrix2x3 localPositionMatrix = UIGradientUtils.LocalPositionMatrix(rect, dir);

            UIVertex vertex = default(UIVertex);
            for (int i = 0; i < vh.currentVertCount; i++) {
                vh.PopulateUIVertex(ref vertex, i);
                Vector2 localPosition = localPositionMatrix * vertex.position;
                vertex.color *= _gradient.Evaluate(localPosition.y);
                vh.SetUIVertex(vertex, i);
            }
        }
    }

}
