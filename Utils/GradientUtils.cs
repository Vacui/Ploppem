using UnityEngine;

/// <summary>
/// Static class with util to create a Gradient.
/// </summary>
public static class GradientUtils {

    public static Gradient GenerateGradient(Color color1, Color color2, float alpha1 = 1, float alpha2 = 1) {
        Gradient result = new Gradient();
        result.SetKeys(
            new GradientColorKey[2] {
                new GradientColorKey(color1, 0),
                new GradientColorKey(color2, 1)
            },
            new GradientAlphaKey[2]{
                new GradientAlphaKey(alpha1,0),
                new GradientAlphaKey(alpha2,1)
            });
        return result;
    }

}
