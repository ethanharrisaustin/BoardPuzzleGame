using MapRooms;
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
        //camMovement = CreateCamMovement();
        camMovingTimer -= Time.deltaTime;

        if (camIsMoving && camMovingTimer < 0)
        {
            camIsMoving = false;

            AudioManager.Stop("Cam Move");
        }
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

    public void MoveCamUp()
    {
        Vector3 camForward = Camera.main.transform.forward;

        camMovement.x = camForward.x;
        camMovement.z = camForward.z;

        transform.position += camMovement * Time.deltaTime * camMoveSpeed;

        CamStartMoving();
    }

    public void MoveCamDown()
    {
        Vector3 camForward = Camera.main.transform.forward;

        camMovement.x = -camForward.x;
        camMovement.z = -camForward.z;

        transform.position += camMovement * Time.deltaTime * camMoveSpeed;

        CamStartMoving();
    }

    public void MoveCamLeft()
    {
        Vector3 camRight = Camera.main.transform.right;

        camMovement.x = -camRight.x;
        camMovement.z = -camRight.z;

        transform.position += camMovement * Time.deltaTime * camMoveSpeed * 0.75f;

        CamStartMoving();
    }

    public void MoveCamRight()
    {
        Vector3 camRight = Camera.main.transform.right;

        camMovement.x = camRight.x;
        camMovement.z = camRight.z;

        transform.position += camMovement * Time.deltaTime * camMoveSpeed * 0.75f;

        CamStartMoving();
    }

    float camMovingTimer = 0f;
    bool camIsMoving = false;
    void CamStartMoving()
    {
        if (camMovingTimer < 0f)
        {
            AudioManager.Play("Cam Move");
        }

        camMovingTimer = 0.2f;
        camIsMoving = true;
    }
}
