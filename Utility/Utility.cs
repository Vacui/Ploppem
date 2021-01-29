using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Utility {

    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void ShuffleList<T>(this IList<T> list, int n = 0) {
        if (n <= 0) n = list.Count;
        while (n > 1) {
            n--;
            int r = Random.Range(0, n + 1);
            T value = list[r];
            list[r] = list[n];
            list[n] = value;
        }
    }

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

    public static T Next<T>(this T src) where T : struct {
        if (!typeof(T).IsEnum) throw new System.ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

        T[] Arr = (T[])System.Enum.GetValues(src.GetType());
        int j = System.Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public static T Prev<T>(this T src) where T : struct {
        if (!typeof(T).IsEnum) throw new System.ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

        T[] Arr = (T[])System.Enum.GetValues(src.GetType());
        int j = System.Array.IndexOf<T>(Arr, src) - 1;
        return (Arr.Length == j) ? Arr[j] : Arr[0];
    }

    public static bool IsPositive(int num) {
        return IsPositive(num);
    }
    public static bool IsPositive(float num) {
        return num > 0;
    }

}

[System.Serializable] public class UnityFloatEvent : UnityEvent<float> { }
[System.Serializable] public class UnityIntEvent : UnityEvent<int> { }