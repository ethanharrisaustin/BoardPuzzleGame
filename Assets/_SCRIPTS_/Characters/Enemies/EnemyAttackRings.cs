using MapRooms;
using UnityEngine;

namespace Combat
{
    public class EnemyAttackRings : MonoBehaviour
    {
        public static EnemyAttackRings instance;

        [SerializeField] ObjectPool objectPool;

        void Awake()
        {
            instance = this;
        }

        public static EnemyAttackRing GetEnemyAttackRing(EnemyGO enemyGO)
        {
            EnemyAttackRing returnValue = null;

            instance.objectPool.LoopThroughActiveObjects((EnemyAttackRing ring) =>
            {
                if (enemyGO == ring.targetEnemy)
                {
                    returnValue = ring;
                }
            });

            if (returnValue != null) return returnValue;

            return instance.objectPool.SpawnObject<EnemyAttackRing>();
        }
    }
}