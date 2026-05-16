using MapRooms;
using UnityEngine;

[RequireComponent(typeof(RoomObjectGO))]
public class FlatObjectWobble : MonoBehaviour
{
    [SerializeField] float wobbleDistance;
    RoomObjectGO roomObjectGO;

    void Awake()
    {
        roomObjectGO = GetComponent<RoomObjectGO>();
    }

    public void DoWobble1()
    {
        transform.position = roomObjectGO.roomObject.position;
        
        FlatObjectWobbleHandler.instance.DoWobble1(transform, wobbleDistance);
    }
}
