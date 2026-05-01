using UnityEngine;

public class CharacterJumpSettings : MonoBehaviour
{
    public static CharacterJumpSettings instance;

    public AnimationCurve jumpYCurve;
    public AnimationCurve jumpXZCurve;
    public AnimationCurve rotationCurve;
    public float jumpTime;
    public float jumpHeight;
    public float rotationAmount;

    void Awake()
    {
        instance = this;
    }

    public static void GetSettings(
        out AnimationCurve jumpYCurve, 
        out AnimationCurve jumpXZCurve, 
        out AnimationCurve rotationCurve, 
        out float jumpTime, 
        out float jumpHeight,
        out float rotationAmount)
    {
        jumpYCurve = instance.jumpYCurve;
        jumpXZCurve = instance.jumpXZCurve;
        rotationCurve = instance.rotationCurve;
        jumpTime = instance.jumpTime;
        jumpHeight = instance.jumpHeight;
        rotationAmount = instance.rotationAmount;
    }
}
