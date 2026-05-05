using UnityEngine;

public class RectExtensions
{
    static Vector3[] cornersA = new Vector3[4];
    static Vector3[] cornersB = new Vector3[4];

    static Rect rect1 = new Rect();
    static Rect rect2 = new Rect();

    public static bool IsOverlapping(RectTransform rectA, RectTransform rectB)
    {
        if (rectA == null || rectB == null) return false;

        rectA.GetWorldCorners(cornersA);
        rectB.GetWorldCorners(cornersB);

        rect1.position = cornersA[0];
        rect1.size = cornersA[2] - cornersA[0];

        rect2.position = cornersB[0];
        rect2.size = cornersB[2] - cornersB[0];

        return rect1.Overlaps(rect2);
    }
}
