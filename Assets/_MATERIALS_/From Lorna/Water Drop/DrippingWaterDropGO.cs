using DG.Tweening;
using MapRooms;
using UnityEngine;

public class DrippingWaterDropGO : RoomObjectGO
{
    [SerializeField] Transform waterDrop, waterDropSpawnPoint;

    [Space]

    [SerializeField] float timeBetweenDrips;
    [SerializeField] float dripTime;
    [SerializeField] Vector3 dropSize;


    public override void Spawn(RoomObject roomObject, RoomObject.FlySettings flySettings)
    {
        base.Spawn(roomObject, flySettings);

        DripWater();
    }

    void OnDisable()
    {
        waterDrop.DOKill(false);
    }

    void DripWater()
    {
        waterDrop.DOKill(false);
        waterDrop.transform.localPosition = waterDropSpawnPoint.localPosition;
        waterDrop.localScale = Vector3.zero;

        float delay = timeBetweenDrips * Random.Range(0.5f, 1.5f);
        float size = Random.Range(0.5f, 1.5f);

        waterDrop.DOScale(dropSize * size, dripTime * 0.25f).SetDelay(delay).SetEase(Ease.InQuad);
        waterDrop.DOLocalMoveY(0f, dripTime).SetDelay(delay).SetEase(Ease.InQuad).OnComplete(DripWater);
    }
}
