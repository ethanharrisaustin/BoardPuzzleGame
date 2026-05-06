using MapRooms;
using UnityEngine;

public class CameraPositionTracker : MonoBehaviour
{
    [Header("Left Righty")]
    [SerializeField] Transform lr_holder;
    [SerializeField] Transform lr_camPositionMarker;
    [SerializeField] Transform lr_topLeftBoundary;
    [SerializeField] Transform lr_bottomRightBoundary;

    [Header("Up Downy")]
    [SerializeField] Transform ud_holder;
    [SerializeField] Transform ud_camPositionMarker;
    [SerializeField] Transform ud_topLeftBoundary;
    [SerializeField] Transform ud_bottomRightBoundary;

    [Space]

    [SerializeField] float leftPadding = 6;
    [SerializeField] float rightPadding = 0;
    [SerializeField] float upPadding = 6;
    [SerializeField] float downPadding = 6;
    [SerializeField] float camZOffset = 15;

    Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        TrackCameraPositionLeftRighty();
        TrackCameraPositionUpDowny();

        Debug.Log(ud_camPositionMarker.localPosition.x - camZOffset + " " + ud_bottomRightBoundary.localPosition.x);
    }

    public bool CanHoverLeft()
    {
        return lr_camPositionMarker.localPosition.x > lr_topLeftBoundary.localPosition.x + leftPadding;
    }

    public bool CanHoverRight()
    {
        return lr_camPositionMarker.localPosition.x < lr_bottomRightBoundary.localPosition.x - rightPadding;
    }

    public bool CanHoverUp()
    {
       return ud_camPositionMarker.localPosition.x - camZOffset > ud_bottomRightBoundary.localPosition.x + upPadding;
    }

    public bool CanHoverDown()
    {
        
        return ud_camPositionMarker.localPosition.x - camZOffset < ud_topLeftBoundary.localPosition.x - downPadding;
    }

    void TrackCameraPositionLeftRighty()
    {
        lr_holder.rotation = cam.transform.rotation;
        lr_camPositionMarker.position = cam.transform.position;

        lr_topLeftBoundary.position = MapRoomSystem.RoomStartPos();
        lr_bottomRightBoundary.position = MapRoomSystem.RoomEndPos();
    }

    void TrackCameraPositionUpDowny()
    {
        ud_holder.rotation = Quaternion.Euler(
            cam.transform.eulerAngles.x, 
            cam.transform.eulerAngles.y + 90f, 
            cam.transform.eulerAngles.z);
         
        ud_camPositionMarker.position = cam.transform.position;

        ud_topLeftBoundary.position = MapRoomSystem.RoomStartPos();
        ud_bottomRightBoundary.position = MapRoomSystem.RoomEndPos();
    }
}
