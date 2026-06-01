using UnityEngine;

namespace MapRooms
{
    public class LevelConnectorGO : RoomObjectGO
    {
        [SerializeField] BoxCollider end1, end2;

        LevelNodeGO levelNodeGO1, levelNodeGO2;

        public override void Init()
        {
            base.Init();

            levelNodeGO1 = GetLevelNodeGO(end1);
            levelNodeGO2 = GetLevelNodeGO(end2);

            if (levelNodeGO1 != null) levelNodeGO1.AddConnectedLevelNode(levelNodeGO2);
            if (levelNodeGO2 != null) levelNodeGO2.AddConnectedLevelNode(levelNodeGO1);
        }

        LevelNodeGO GetLevelNodeGO(BoxCollider collider)
        {
            Collider[] colliders = Overlapping(collider);

            for (int i = 0; i < colliders.Length; ++i)
            {
                LevelNodeGO levelNodeGO = colliders[i].GetComponentInParent<LevelNodeGO>();

                if (levelNodeGO == null) continue;

                return levelNodeGO;
            }

            return null;
        }

        public override string ObjectFlyInCategory()
        {
            return "LevelConnectGO";
        }
    }
}