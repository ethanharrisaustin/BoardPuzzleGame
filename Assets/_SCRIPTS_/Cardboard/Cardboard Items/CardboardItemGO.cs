using UnityEngine;
using MapRooms;
using MapNavigation;
using UnityEngine.Events;

namespace Cardboard
{
    public class CardboardItemGO : RoomObjectGO, IButton3D, IDraggable3D
    {
        public CardboardItemObject cardboardItemObject;

        public bool dontCollect = false;

        public void MouseOver()
        {
    
        }

        public void MouseOut()
        {

        }

        public void Click()
        {
            if (dontCollect) return;
             
            UI_ItemBoard.AddItemToBoard(cardboardItemObject);

            Destroy(gameObject);
        }

        public override string ObjectFlyInCategory()
        {
            return "Items";
        }

        public string[] GetTurnSlotsIDS()
        {
            return cardboardItemObject.GetTurnSlotsIDS();
        }

        protected override void Update()
        {
            base.Update();

            if (Mathf.Abs(transform.localScale.x) < 0.05f)
            {
                gameObject.SetActive(false);
            }
        }

        void OnDisable()
        {
            AudioSource[] audios = GetComponentsInChildren<AudioSource>();

            for (int i = 0; i < audios.Length; ++i) audios[i].Stop();            
        }

        public void MouseDown()
        {
            
        }

        public void MouseUp()
        {
            
        }

        public void StartDrag(Vector2 mousePos)
        {
            UI_ItemBoard.StartDraggingItem(cardboardItemObject);

            Destroy(gameObject);
        }

        public void OnDrag(Vector2 mousePos)
        {
            
        }

        public void EndDrag(Vector2 mousePos)
        {
            UI_ItemBoard.StopDraggingItem(cardboardItemObject);
        }
    }
}