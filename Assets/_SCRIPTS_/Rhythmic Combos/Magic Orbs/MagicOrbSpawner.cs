using System.Collections.Generic;
using MapNavigation;
using UnityEngine;
using MapRooms;

namespace Combat
{
    public class MagicOrbSpawner : MonoBehaviour
    {
        public ObjectPool objectPool;

        public static MagicOrbSpawner instance;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void SpawnMagicOrbs(CombatAttack combatAttack, ObjectWithHealthGO targetObject = null)
        {
            if (targetObject == null) targetObject = ClosestEnemy();

            for (int i = 0; i < combatAttack.inputs.Count; ++i)
            {
                MagicOrb magicOrb = objectPool.SpawnObject().GetComponent<MagicOrb>();

                int _i;
                Color magicColour;
                CombatColours.GetElementFromInput(combatAttack.inputs[i].comboButtonIndexes[0], out _i, out _, out magicColour, out _);

                magicOrb.Spawn(magicColour, targetObject, combatAttack.inputs[i].accuracy, (float)i * 0.2f);
            }
        }

        EnemyGO ClosestEnemy()
        {
            EnemyGO[] enemyGOs = FindObjectsByType<EnemyGO>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            EnemyGO c_enemy = null;
            float c_distance = Mathf.Infinity;

            for (int i = 0; i < enemyGOs.Length; ++i)
            {
                float new_distance = Vector3.Distance(PlayerGO.instance.GetPosition(), enemyGOs[i].GetPosition());

                if (new_distance < c_distance)
                {
                    c_distance = new_distance;
                    c_enemy = enemyGOs[i];
                }
            }

            return c_enemy;
        }
    }
}
