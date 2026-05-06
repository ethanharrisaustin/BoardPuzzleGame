using UnityEngine;

public class CameraMoveWithMouse : MonoBehaviour
{
    CameraMouseUITriggers mouseTriggers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouseTriggers = CameraMouseUITriggers.instance;
    }

    [SerializeField] float camMoveSpeed = 1f;
    [SerializeField] Transform camFowardLeft, camFowardRight;
    Vector3 camMovement;
    
    // Update is called once per frame
    void Update()
    {
        camMovement = CreateCamMovement();

        transform.position += camMovement * Time.deltaTime * camMoveSpeed;
    }

    Vector3 cachedCamMovement = new Vector3();
    Vector3 CreateCamMovement()
    {
        cachedCamMovement.x = 0f;
        cachedCamMovement.z = 0f;

        if (TopLeftCorner()) MoveTopLeft();

        else if (TopRightCorner()) MoveTopRight();

        else if (BottomLeftCorner()) MoveBottomLeft();

        else if (BottomRightCorner()) MoveBottomRight();
        
        else
        {
            MoveLeftOrRight();

            MoveUpOrDown();
        }

        cachedCamMovement.y = 0f;

        cachedCamMovement.Normalize();

        return cachedCamMovement;
    }

    bool TopLeftCorner()
    {
        return mouseTriggers.MouseAtLeft && mouseTriggers.MouseAtUp;
    }

    bool TopRightCorner()
    {
        return mouseTriggers.MouseAtRight && mouseTriggers.MouseAtUp;
    }

    bool BottomLeftCorner()
    {
        return mouseTriggers.MouseAtLeft && mouseTriggers.MouseAtDown;
    }

    bool BottomRightCorner()
    {
        return mouseTriggers.MouseAtRight && mouseTriggers.MouseAtDown;
    }

    void MoveTopLeft()
    {
        cachedCamMovement.x = camFowardLeft.forward.x;
        cachedCamMovement.z = camFowardLeft.forward.z;        
    }

    void MoveTopRight()
    {
        cachedCamMovement.x = camFowardRight.forward.x;
        cachedCamMovement.z = camFowardRight.forward.z;        
    }

    void MoveBottomLeft()
    {
        cachedCamMovement.x = -camFowardRight.forward.x;
        cachedCamMovement.z = -camFowardRight.forward.z;        
    }

    void MoveBottomRight()
    {
        cachedCamMovement.x = -camFowardLeft.forward.x;
        cachedCamMovement.z = -camFowardLeft.forward.z;        
    }

    void MoveLeftOrRight()
    {
        Vector3 camRight = Camera.main.transform.right;

        if (mouseTriggers.MouseAtLeft)
        {
            cachedCamMovement.x = -camRight.x;
            cachedCamMovement.z = -camRight.z;
        }
        else if (mouseTriggers.MouseAtRight)
        {
            cachedCamMovement.x = camRight.x;
            cachedCamMovement.z = camRight.z;
        }
    }

    void MoveUpOrDown()
    {
        Vector3 camForward = Camera.main.transform.forward;

        if (mouseTriggers.MouseAtDown)
        {
            cachedCamMovement.x = -camForward.x;
            cachedCamMovement.z = -camForward.z;
        }
        else if (mouseTriggers.MouseAtUp)
        {
            cachedCamMovement.x = camForward.x;
            cachedCamMovement.z = camForward.z;
        }
    }


}
