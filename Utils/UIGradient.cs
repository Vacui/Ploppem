using UnityEngine;
using UnityEngine.UI;

public class UIGradient : BaseMeshEffect
{
    [SerializeField] Gradient _gradient = new Gradient();
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

    public void SetGradient(Gradient newGradient) {
        _gradient = newGradient;
        GetComponent<Image>().enabled = false;
        GetComponent<Image>().enabled = true;
    }

    public Gradient GetGradient() {
        return _gradient;
    }
}
