using BoardGame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoveItMoveIt
{
    public class UI_Card_Base : UI_Item_Base
    {
        public virtual bool PerformAction(BoardGamePlayer player)
        {
            if (player.GetCardboardHolder().shrunken) return false;
            
            AudioManager.Play3D("Jump V1", player.GetCardboardHolder().GetPosition());

            return true;
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