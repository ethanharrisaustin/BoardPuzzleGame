using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoveItMoveIt
{
    public class UI_Card_Base : UI_Item_Base
    {
        public virtual void PerformAction()
        {
            
        }

        public override void OnDrag(UI_DraggedItem draggedItem)
        {
            UI_DraggedCardHitpoints.instance.FollowDraggedItem(draggedItem);
        }

        public override bool CancelDrag()
        {
            if (UI_TurnSlot.hoveredSlot != null)
                return false;
            
            if (UI_TurnSlot.cardRectHoveredSlot != null)
                return false;
            
            return true;
        }

        public override bool AddToTurnSlot()
        {
            return UI_TurnSlot.hoveredSlot != null || UI_TurnSlot.cardRectHoveredSlot != null;
        }
    }
}