using System.Collections.Generic;
using MapRooms;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    static List<CameraTarget> activeCameraTargets = new List<CameraTarget>();

    public int priority = 0;
    public bool alsoControlRotation = true;

    void OnEnable()
    {
        if (!activeCameraTargets.Contains(this)) activeCameraTargets.Add(this);
    }

    void OnDisable()
    {
        activeCameraTargets.Remove(this);
    }

    public static Vector3 GetCameraTargetPosition(Vector3 camCurrentPosition)
    {
        CameraTarget cameraTarget = GetCameraTarget();

        if (cameraTarget == null) return camCurrentPosition;

        return cameraTarget.transform.position;
    }

    public static Quaternion GetCameraTargetRotation(Quaternion camCurrentRotation)
    {
        CameraTarget cameraTarget = GetCameraTarget();

        if (cameraTarget == null || cameraTarget.alsoControlRotation == false) return camCurrentRotation;

        return cameraTarget.transform.rotation;
    }
    
    static CameraTarget GetCameraTarget()
    {
        int largestPriority = -9999999;
        CameraTarget c_cameraTarget = null;

        for (int i = 0; i < activeCameraTargets.Count;)
        {
            if (activeCameraTargets[i] == null)
            {
                activeCameraTargets.RemoveAt(i);
                continue;
            }

            if (activeCameraTargets[i].priority > largestPriority)
            {
                c_cameraTarget = activeCameraTargets[i];

                largestPriority = activeCameraTargets[i].priority;
            }

            ++i;
        }

        return c_cameraTarget;
    }
}
