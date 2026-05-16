using DG.Tweening;
using UnityEngine;

public class FlatObjectWobbleHandler : MonoBehaviour
{
    public static FlatObjectWobbleHandler instance;

    [Header("Wobble 1")]
    [Space]
    [SerializeField] AnimationCurve wobble1YCurve;
    [SerializeField]  AnimationCurve wobble1XZCurve;
    [Space]
    [SerializeField] float wobble1Time;

    [Space]
    [Header("Wobble 2")]
    [Space]
    [SerializeField]  AnimationCurve wobble2YCurve;
    [SerializeField]  AnimationCurve wobble2XCurve;
    [SerializeField]  AnimationCurve wobble2ZCurve;
    [Space]
    [SerializeField]  float wobble2Time;

    [Space]
    [Header("Bounce")]
    [Space]
    [SerializeField]  AnimationCurve bounce1YCurve;
    [SerializeField]  AnimationCurve bounce1ZCurve;
    [SerializeField]  AnimationCurve bounce1XCurve;
    [Space]
    [SerializeField]  float bounce1Time;

    void Awake()
    {
        instance = this;
    }

    public void DoWobble1(Transform transform, float distance)
    {
        Vector3 startPos = transform.position;
        Vector3 startRot = transform.eulerAngles;

        transform.DOKill(false);
        transform.DOMoveY(transform.position.y + distance, wobble1Time).SetEase(wobble1YCurve);

        Vector3 rotateDist = transform.eulerAngles + new Vector3(distance * Random.Range(0.2f, 1f), 0f, distance * Random.Range(0.2f, 1f)) * 100f;

        transform.DORotate(rotateDist, wobble1Time).SetEase(wobble1XZCurve).OnComplete(() =>
        {
            transform.position = startPos;
            transform.eulerAngles = startRot; 
        });
    }
}
