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

        public virtual void Remove(RoomObject.FlySettings flySettings, Action<RoomObjectGO> destroy)
        {
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

        void FlyObjectOut(RoomObject.FlySettings flySettings, Action<RoomObjectGO> destroy)
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
                .OnComplete(() => { flyingOut = false; destroy.Invoke(this); });
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
            OnWasDeactivated();

            MapRoomSystem.OnRoomObjectWasDeactivated();
        }

        void OnWasDeactivated()
        {
            if (roomObjectPool == null) return;

            roomObjectPool.needsToRecalulateActives = true;
        }

        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            
        }

        public virtual void Init()
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
    }
}