using DG.Tweening;
using UnityEngine;

public class UI_PopUpMenu : MonoBehaviour
{
    RectTransform rectTransform;
    Vector2 targetSizeDelta;

    PopUpMenuAnimationCurves curves;

    bool calledAwake = false;

    void Awake()
    {
        CloseImmediately();

        rectTransform = GetComponent<RectTransform>();
        targetSizeDelta = rectTransform.sizeDelta;

        calledAwake = true;
    }

    bool calledStart = false;
    void Start()
    {
        curves = PopUpMenuAnimationCurves.instance;

        calledStart = true;
    }

    public void Open()
    {
        if (!calledAwake) Awake();
        if (!calledStart) Start();

        gameObject.SetActive(true);

        rectTransform.DOKill(false);

        rectTransform.sizeDelta = Vector2.up * targetSizeDelta;
        rectTransform.DOSizeDelta(targetSizeDelta, curves.openTime).SetEase(curves.openX).SetUpdate(true);

        rectTransform.localScale = Vector2.right;
        rectTransform.DOScale(1f, curves.openTime).SetEase(curves.openY).SetUpdate(true);
    }

    public void Close()
    {
        rectTransform.DOKill(false);

        rectTransform.DOSizeDelta(Vector2.up * targetSizeDelta, curves.closeTime).SetEase(curves.closeX).SetUpdate(true);

        rectTransform.DOScale(Vector2.right, curves.closeTime).SetEase(curves.closeY).SetUpdate(true).OnComplete(CloseImmediately);
    }

    public void CloseImmediately()
    {
        gameObject.SetActive(false);
    }
}
