using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smoothing = 0.5f;

    Vector3 refVelocity = Vector2.zero;
    Vector3 refEulerAngles = Vector3.zero;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = CameraTarget.GetCameraTargetPosition(transform.position);
        Quaternion targetRotation = CameraTarget.GetCameraTargetRotation(transform.rotation);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref refVelocity, smoothing, 9999f, Time.smoothDeltaTime);

        //transform.eulerAngles = Vector3.SmoothDamp(transform.position, targetRotation.eulerAngles, ref refEulerAngles, smoothing, 9999f, Time.smoothDeltaTime);
        
        float newRotX = Mathf.SmoothDampAngle(transform.eulerAngles.x, targetRotation.eulerAngles.x, ref refEulerAngles.x, smoothing, 9999f, Time.smoothDeltaTime);
        float newRotY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, ref refEulerAngles.y, smoothing, 9999f, Time.smoothDeltaTime);
        float newRotZ = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetRotation.eulerAngles.z, ref refEulerAngles.z, smoothing, 9999f, Time.smoothDeltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.smoothDeltaTime / smoothing * 1.2f);

        transform.eulerAngles = new Vector3(newRotX, newRotY, newRotZ);
    }
}
