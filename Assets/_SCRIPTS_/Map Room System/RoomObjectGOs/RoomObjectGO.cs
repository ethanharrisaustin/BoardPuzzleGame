using UnityEngine;
using DG.Tweening;
using System;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MapRooms
{
    public class RoomObjectGO : MonoBehaviour
    {
        [HideInInspector] public RoomObject roomObject;
        [HideInInspector] public string[] objectValues;
        [HideInInspector] public RoomObjectPool roomObjectPool;


        #if UNITY_EDITOR
        public RoomObject GetRoomObject()
        {
            if (IgnoreInRoomMaking()) return null;

            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject);

            if (prefab == null)
            {
                Debug.Log("Error! One of the RoomObjects' is not a prefab!");
                return null;
            }

            string[] values;
            GetValues(out values);

            return new RoomObject(prefab, transform.position, transform.localScale, transform.eulerAngles, values);
        }
        #endif

        public bool FinishedFlyingIn()
        {
            if (gameObject.activeSelf == false) return true;

            return flyingIn == false;
        }

        public bool FinishedFlyingOut()
        {
            return flyingOut == false || gameObject.activeSelf == false;
        }

        [HideInInspector] public Vector3 targetPosition;
        bool flyingIn = false;
        /// <summary>
        /// Called as the spawn ('fall in') animation starts.
        /// Make sure to call base.Spawn() otherwise they won't
        /// fall into the scene!
        /// </summary>
        public virtual void Spawn(RoomObject roomObject, RoomObject.FlySettings flySettings)
        {
            transform.DOKill(false);

            transform.localScale = roomObject.scale;
            transform.eulerAngles = roomObject.rotation;

            targetPosition = roomObject.position;
            FlyObjectIn(targetPosition, flySettings);

            this.roomObject = roomObject;

            SetValues(roomObject.values);
        }

        /// <summary>
        /// Called as the remove ('fly out') animation starts.
        /// Make sure to call base.Remove() otherwise they won't
        /// transition out.
        /// </summary>
        public virtual void Remove(RoomObject.FlySettings flySettings, Action<RoomObjectGO> destroy)
        {
            if (IgnoreInRoomMaking()) return;

            transform.DOKill(false);
            
            FlyObjectOut(flySettings, destroy);
        }

        void FlyObjectIn(Vector3 targetPosition, RoomObject.FlySettings flySettings)
        {
            // If not in play mode
            if (MapRoomSystem.instance == null)
            {
                transform.position = targetPosition;
                return;
            }

            flyingIn = true;

            transform.position = targetPosition + Vector3.up * flySettings.startYPos;

            float delay = (transform.position.x + transform.position.z + 5) * flySettings.delayMultiplier;
            delay += flySettings.initialDelay;

            transform.DOMoveY(targetPosition.y, flySettings.fallTime)
                .SetEase(flySettings.curve)
                .SetDelay(delay)
                .OnComplete(() => flyingIn = false);
        }

        bool flyingOut= false;

        void FlyObjectOut(RoomObject.FlySettings flySettings, Action<RoomObjectGO> onDestroy)
        {
            // REALLY DIRTY FIX
            if (flySettings == null)
            {
                gameObject.SetActive(false);
                return;
            }

            flyingOut = true;

            float delay = (transform.position.x + transform.position.z + 5) * flySettings.delayMultiplier;
            delay += flySettings.initialDelay;

            transform.DOMoveY(transform.position.y + flySettings.startYPos, flySettings.fallTime)
                .SetEase(flySettings.curve)
                .SetDelay(delay)
                .OnComplete(() => { flyingOut = false; onDestroy.Invoke(this); });
        }

        public virtual string ObjectFlyInCategory()
        {
            return "RoomObjectGO";
        }

        public static RoomObjectGO GetRoomObjectGO(Collider collider)
        {
            RoomObjectGO roomObjectGO = collider.GetComponentInChildren<RoomObjectGO>();
            if (roomObjectGO == null) roomObjectGO = collider.GetComponentInParent<RoomObjectGO>();

            return roomObjectGO;
        }

        void OnDisable()
        {
            if (IgnoreInRoomMaking()) return;

            OnWasDeactivated();

            MapRoomSystem.OnRoomObjectWasDeactivated();
        }

        void OnWasDeactivated()
        {
            if (IgnoreInRoomMaking()) return;

            if (roomObjectPool == null) return;

            roomObjectPool.needsToRecalulateActives = true;
        }

        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            
        }

        /// <summary>
        /// Called once all of the fall in animations from Spawn() have finished,
        /// good for doing checks that require all objects in the level to be in their final 
        /// spawn positions.
        /// </summary>
        public virtual void Init()
        {
            
        }

        /// <summary>
        /// Called one frame after Init().
        /// </summary>
        public virtual void LateInit()
        {
            
        }

        protected virtual void Update()
        {
            
        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        public virtual void GetValues(out string[] values)
        {
            values = null;
        }

        public virtual void SetValues(string[] values)
        {

        }

        public virtual Vector3 GetPosition()
        {
            return transform.position;
        }
        public virtual Vector3 GetCenterPosition()
        {
            return transform.position + Vector3.up * 0.8f;
        }
        public virtual Vector3 GetEulerAngles()
        {
            return transform.eulerAngles;
        }
        public virtual Quaternion GetRotation()
        {
            return transform.rotation;
        }
        public virtual Vector3 GetScale()
        {
            return transform.localScale;
        }
        public virtual Vector3[] GetAdjacentCardinalTiles()
        {
            return new Vector3[]
            {
                GetPosition() + Vector3.left,
                GetPosition() + Vector3.forward,
                GetPosition() + Vector3.right,
                GetPosition() + Vector3.back,
            };
        }
        public virtual Vector3[] GetAdjacentDiagonalTiles()
        {
            return new Vector3[]
            {
                GetPosition() + Vector3.left + Vector3.forward,
                GetPosition() + Vector3.right + Vector3.forward,
                GetPosition() + Vector3.right + Vector3.back,
                GetPosition() + Vector3.left + Vector3.back,
            };
        }
        public virtual Vector3[] GetAdjacentTiles()
        {
            return new Vector3[]
            {
                GetPosition() + Vector3.left,
                GetPosition() + Vector3.left + Vector3.forward,
                GetPosition() + Vector3.forward,
                GetPosition() + Vector3.right + Vector3.forward,
                GetPosition() + Vector3.right,
                GetPosition() + Vector3.right + Vector3.back,
                GetPosition() + Vector3.back,
                GetPosition() + Vector3.left + Vector3.back,
            };
        }

        public virtual bool GetRoomObjectSave(out RoomObjectSave roomObjectSave)
        {
            roomObjectSave = null;
            return false;
        }

        public virtual string[] GetSaveValues()
        {
            return null;
        }

        public virtual void LoadRoomObject(RoomObjectSave roomObjectSave)
        {
            
        }

        public virtual bool DoPooling()
        {
            return true;
        }

        public static bool Matching(RoomObjectGO a, RoomObjectGO b)
        {
            if (a == null || b == null) return false;

            return a.GetPosition() == b.GetPosition() && a.GetScale() == b.GetScale() && a.GetRotation() == b.GetRotation();
        }

        public virtual bool IgnoreInRoomMaking()
        {
            return false;
        }

        Vector3 BoxCenter(BoxCollider collider)
        {
            return collider.transform.position + collider.center;
        }

        Vector3 BoxHalfExtents(BoxCollider collider)
        {
            return new Vector3(
                collider.size.x * collider.transform.lossyScale.x,
                collider.size.y * collider.transform.lossyScale.y,
                collider.size.z * collider.transform.lossyScale.z
                );
        }

        /// <summary>
        /// Returns an array of colliders that are inside the given box collider.
        /// </summary>
        protected Collider[] Overlapping(BoxCollider collider)
        {
            return Physics.OverlapBox(
                BoxCenter(collider), 
                BoxHalfExtents(collider), 
                collider.transform.rotation, 
                ~0, 
                QueryTriggerInteraction.Collide);
        }
    }
}