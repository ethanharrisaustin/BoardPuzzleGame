using DG.Tweening;
using MapNavigation;
using UnityEngine;

namespace MapRooms
{
    public class LevelSelectorGO : RoomObjectGO
    {
        [SerializeField] Transform graphic;
        [SerializeField] float spinSpeed;
        [SerializeField] float bounceTime, bounceHeight;
        [SerializeField] AnimationCurve bounceCurve;

        [SerializeField] BoxCollider boxCollider;

        public static bool isTweening {get { return tweeningTimer > 0f;}}

        static float tweeningTimer = 0f;

        LevelNodeGO selectedLevel = null;

        protected override void Update()
        {
            base.Update();

            graphic.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);

            tweeningTimer -= Time.unscaledDeltaTime;

            if (Input.submit)
            {
                SelectLevel();
            }
        }

        public override void Spawn(RoomObject roomObject, RoomObject.FlySettings flySettings)
        {
            base.Spawn(roomObject, flySettings);

            StartBouncing();
        }

        public override void Init()
        {
            base.Init();

            selectedLevel = GetOverlappingLevelNode();

            if (selectedLevel != null)
            {
                DisplaySelectedLevel();
            }
        }

        LevelNodeGO GetOverlappingLevelNode()
        {
            Collider[] colliders = Overlapping(boxCollider);
            LevelNodeGO selectedLevel;

            for (int i = 0; i < colliders.Length; ++i)
            {
                selectedLevel = colliders[i].GetComponentInParent<LevelNodeGO>();

                if (selectedLevel != null)
                {
                    return selectedLevel;
                }
            }

            return null;
        }

        void StartBouncing()
        {
            graphic.DOKill(false);

            graphic.localPosition = Vector3.zero;
            graphic.DOLocalMoveY(bounceHeight, bounceTime).SetEase(bounceCurve).OnComplete(StartBouncing);
        }

        void OnDisable()
        {
            graphic.DOKill(false);
        }

        public void MoveTo(LevelNodeGO levelNodeGO)
        {
            tweeningTimer = 2f;

            transform.DOMove(
                new Vector3(levelNodeGO.transform.position.x, transform.position.y, levelNodeGO.transform.position.z), 0.3f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => { tweeningTimer = -1f; });
            
            selectedLevel = levelNodeGO;

            DisplaySelectedLevel();
        }

        public override string ObjectFlyInCategory()
        {
            return "LevelNodeGO";
        }

        void DisplaySelectedLevel()
        {
            UI_LevelSelect.Get().ShowLevel(selectedLevel);
        }

        void SelectLevel()
        {
            if (selectedLevel == null) 
            {
                Debug.Log("selectedLevel is null.");

                selectedLevel = GetOverlappingLevelNode();

                if (selectedLevel == null)
                {
                    Debug.Log("Did not find overklapp9hg level");
                    return;
                }
            }

            if (MapRoomSystem.instance.levelSelectGO.activeSelf == false) return;

            if (selectedLevel.Unlocked() == false) return;

            string id = selectedLevel.level_id;

            MapRoomSystem.instance.ShowInLevel();

            MapRoomSystem.instance.SwapToRoom(id);
        }
    }
}