using System;
using DG.Tweening;
using UnityEngine;

public static class DoTweenExtensions
{
    public static void DORotateAround(
        Transform transform, 
        Vector3 pivotWorldSpace, 
        Vector3 rotateAxis, 
        float targetAngle, 
        float time, 
        Ease ease = Ease.Linear, 
        float delay = 0f,
        Action onComplete = null
        )
    {
        float previousAngle = 0f;

        DOVirtual.Float(0f, targetAngle, time, (float value) =>
        {
            float delta = targetAngle - previousAngle;

            transform.RotateAround(pivotWorldSpace, rotateAxis, delta);

            previousAngle = targetAngle;
        })
        .SetDelay(delay).SetEase(ease).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public static void DORotateAround(
        Transform transform, 
        Vector3 pivotWorldSpace, 
        Vector3 rotateAxis, 
        float targetAngle, 
        float time, 
        AnimationCurve animationCurve, 
        float delay = 0f,
        Action onComplete = null
        )
    {
        float previousAngle = 0f;

        DOVirtual.Float(0f, targetAngle, time, (float value) =>
        {
            float delta = value - previousAngle;

            transform.RotateAround(pivotWorldSpace, Vector3.up, delta);

            previousAngle = value;
        })
        .SetDelay(delay).SetEase(animationCurve).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
