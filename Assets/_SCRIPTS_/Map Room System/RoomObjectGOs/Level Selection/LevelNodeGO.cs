using System.Collections.Generic;
using Saving;
using UnityEngine;

namespace MapRooms
{
    public class LevelNodeGO : RoomObjectGO
    {
        public string level_id;
        public bool isBonusLevel;

        [HideInInspector] [SerializeField] BoxCollider boxCollider;

        [SerializeField] Material locked, unlocked, completed, bonusLevel, bonusCompleted;

        [HideInInspector] [SerializeField] Renderer lightRenderer;

        new Light light;

        [HideInInspector] public List<LevelNodeGO> connectedLevelNodes = new List<LevelNodeGO>();

        public override void Spawn(RoomObject roomObject, RoomObject.FlySettings flySettings)
        {
            base.Spawn(roomObject, flySettings);

            connectedLevelNodes.Clear();

            light = GetComponentInChildren<Light>();
        }

        public void AddConnectedLevelNode(LevelNodeGO levelNodeGO)
        {
            if (levelNodeGO == null) return;

            if (LevelNodeAlreadyInConnectedNodesList(levelNodeGO)) return;

            connectedLevelNodes.Add(levelNodeGO);
        }

        public override void LateInit()
        {
            base.LateInit();

            if (Completed())
            {
                

                if (isBonusLevel)
                {
                    light.color = Color.cyan;
                    lightRenderer.material = bonusCompleted;
                }
                else
                {
                    light.color = Color.green;
                    lightRenderer.material = completed;
                }
            }
            else if (Unlocked())
            {
                if (isBonusLevel)
                {
                    light.color = Color.blue;
                    lightRenderer.material = bonusLevel;
                }
                else
                {
                    light.color = Color.white;
                    lightRenderer.material = unlocked;
                }
            }
            else
            {
                lightRenderer.material = locked;

                light.color = Color.white * 0.6f;
            }
        }

        bool LevelNodeAlreadyInConnectedNodesList(LevelNodeGO levelNodeGO)
        {
            for (int i = 0; i < connectedLevelNodes.Count; ++i)
            {
                if (connectedLevelNodes[i].level_id == levelNodeGO.level_id) return true;
            }

            return false;
        }

        public bool Unlocked()
        {
            if (level_id == "1") return true;

            for (int i = 0; i < connectedLevelNodes.Count; ++i)
            {
                if (connectedLevelNodes[i].Completed()) return true;
            }

            return false;
        }

        public bool Completed()
        {
            return Main.GetSaveManager().GetBool(level_id + "completed");
        }

        public void SetCompleted()
        {
            Main.GetSaveManager().SetBool(level_id + "completed", true);
        }

        public override void GetValues(out string[] values)
        {
            string bonusLevelString = isBonusLevel ? "1" : "0";

            values = new string[] { level_id, bonusLevelString };
        }

        public override void SetValues(string[] values)
        {
            if (values == null || values.Length != 2)
            {
                Debug.Log("Buttocks");
            }

            level_id = values[0];
            isBonusLevel = values[1] == "1";
        }

        protected override void Update()
        {
            base.Update();

            if (LevelSelectorGO.isTweening) return;

            Input.Direction direction = Input.MovementAsDirection();

            if (direction != Input.Direction.none && SelectorIsOnNode(out var levelSelectorGO))
            {
                MoveLevelSelector(direction, levelSelectorGO);
            }
        }

        void MoveLevelSelector(Input.Direction direction, LevelSelectorGO levelSelectorGO)
        {
            switch(direction)
            {
                case Input.Direction.north:
                    MoveUp(levelSelectorGO);
                    break;
                case Input.Direction.east:
                    MoveRight(levelSelectorGO);
                    break;
                case Input.Direction.south:
                    MoveDown(levelSelectorGO);
                    break;
                case Input.Direction.west:
                    MoveLeft(levelSelectorGO);
                    break;
            }
        }

        bool SelectorIsOnNode(out LevelSelectorGO levelSelectorGO)
        {
            Collider[] colliders = Overlapping(boxCollider);

            for (int i = 0; i < colliders.Length; ++i)
            {
                levelSelectorGO = colliders[i].GetComponentInParent<LevelSelectorGO>();
                if (levelSelectorGO != null) return true;       
            }

            levelSelectorGO = null;
            return false;
        }

        void MoveUp(LevelSelectorGO levelSelectorGO)
        {
            if (!TryGetLevelNodeUp(out var levelNodeGO)) return;

            levelSelectorGO.MoveTo(levelNodeGO);
        }

        bool TryGetLevelNodeUp(out LevelNodeGO levelNodeGO)
        {
            float highestZPos = Mathf.NegativeInfinity;
            levelNodeGO = null;

            for (int i = 0; i < connectedLevelNodes.Count; ++i)
            {
                Vector3 pos = connectedLevelNodes[i].transform.position;

                if (pos.z < transform.position.z + 0.1f) continue;

                if (pos.z <= highestZPos) continue;

                highestZPos = connectedLevelNodes[i].transform.position.z;
                levelNodeGO = connectedLevelNodes[i];
            }

            return levelNodeGO != null;
        }

        void MoveRight(LevelSelectorGO levelSelectorGO)
        {
            if (!TryGetLevelNodeRight(out var levelNodeGO)) return;

            levelSelectorGO.MoveTo(levelNodeGO);
        }

        bool TryGetLevelNodeRight(out LevelNodeGO levelNodeGO)
        {
            float highestXPos = Mathf.NegativeInfinity;
            levelNodeGO = null;

            for (int i = 0; i < connectedLevelNodes.Count; ++i)
            {
                Vector3 pos = connectedLevelNodes[i].transform.position;

                if (pos.x < transform.position.x + 0.1f) continue;

                if (pos.x <= highestXPos) continue;
                
                highestXPos = connectedLevelNodes[i].transform.position.x;
                levelNodeGO = connectedLevelNodes[i];
            }

            return levelNodeGO != null;
        }

        void MoveDown(LevelSelectorGO levelSelectorGO)
        {
            if (!TryGetLevelNodeDown(out var levelNodeGO)) return;

            levelSelectorGO.MoveTo(levelNodeGO);
        }

        bool TryGetLevelNodeDown(out LevelNodeGO levelNodeGO)
        {
            float smallestZPos = Mathf.Infinity;
            levelNodeGO = null;

            for (int i = 0; i < connectedLevelNodes.Count; ++i)
            {
                Vector3 pos = connectedLevelNodes[i].transform.position;

                if (pos.z > transform.position.z - 0.1f) continue;

                if (pos.z >= smallestZPos) continue;
                
                smallestZPos = connectedLevelNodes[i].transform.position.z;
                levelNodeGO = connectedLevelNodes[i];
            }

            return levelNodeGO != null;
        }

        void MoveLeft(LevelSelectorGO levelSelectorGO)
        {
            if (!TryGetLevelNodeLeft(out var levelNodeGO)) return;

            levelSelectorGO.MoveTo(levelNodeGO);
        }

        bool TryGetLevelNodeLeft(out LevelNodeGO levelNodeGO)
        {
            float smallestXPos = Mathf.Infinity;
            levelNodeGO = null;

            for (int i = 0; i < connectedLevelNodes.Count; ++i)
            {
                Vector3 pos = connectedLevelNodes[i].transform.position;

                if (pos.x > transform.position.x - 0.1f) continue;

                if (pos.x >= smallestXPos) continue;
                
                smallestXPos = connectedLevelNodes[i].transform.position.x;
                levelNodeGO = connectedLevelNodes[i];
            }

            return levelNodeGO != null;
        }

        public override string ObjectFlyInCategory()
        {
            return "LevelNodeGO";
        }
        
    }
}