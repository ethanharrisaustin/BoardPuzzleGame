using UnityEngine;
using UnityEngine.UI;

namespace Cardboard
{
    public static class CardboardExtras
    {
        public static void MatchImageAToB(Component a, Component b)
        {
            MatchImageAToB(a.transform, b.transform);
        }

        public static void MatchImageAToB(Transform a, Transform b)
        {
            Image[] arrayA = a.GetComponentsInChildren<Image>();
            Image[] arrayB = b.GetComponentsInChildren<Image>();

            int loopLength = Mathf.Min(arrayA.Length, arrayB.Length);
            int maxLength = Mathf.Max(arrayA.Length, arrayB.Length);          

            for (int i = 0; i < loopLength; ++i)
            {
                arrayA[i].enabled = arrayB[i].enabled;
                MatchImageAToB(arrayA[i], arrayB[i]);
            }

            for (int i = loopLength; i < maxLength; ++i)
            {
                if (i < arrayA.Length) arrayA[i].enabled = false;

                if (i < arrayB.Length) arrayB[i].enabled = false;
            }
        }

        public static void MatchImageAToB(Image a, Image b)
        {
            // Size
            a.transform.localScale = b.transform.localScale;
            a.transform.rotation = b.transform.rotation;
            a.transform.localPosition = b.transform.localPosition;
            a.rectTransform.sizeDelta = b.rectTransform.sizeDelta;

            // Image and Colour
            a.sprite = b.sprite;
            a.color = b.color;
            a.type = b.type;
            a.pixelsPerUnitMultiplier = b.pixelsPerUnitMultiplier;
        }
    }
}