using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smoothing = 0.5f;

    Vector3 refVelocity = Vector2.zero;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = CameraTarget.GetCameraTargetPosition(transform.position);
        Quaternion targetRotation = CameraTarget.GetCameraTargetRotation(transform.rotation);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref refVelocity, smoothing, 9999f, Time.smoothDeltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.smoothDeltaTime / smoothing);
    }
}
