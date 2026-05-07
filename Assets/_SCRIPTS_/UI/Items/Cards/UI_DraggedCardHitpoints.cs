using System.Collections.Generic;
using UnityEngine;
using MoveItMoveIt;

public class UI_DraggedCardHitpoints : MonoBehaviour
{
    public static UI_DraggedCardHitpoints instance;

    void Awake()
    {
        instance = this;
    }

    public void FollowDraggedItem(UI_DraggedItem draggedItem)
    {
        transform.position = draggedItem.transform.position;

        UI_TurnSlot.CardRectExit();

        List<UI_TurnSlot> hoveredOverSlots = UI_TurnSlot.OverlappingSlots(draggedItem);

        if (hoveredOverSlots.Count == 0) return;

        UI_TurnSlot closestSlot = ClosestToMouse(hoveredOverSlots);

        closestSlot.CardRectEnter();
    }

    UI_TurnSlot ClosestToMouse(List<UI_TurnSlot> slots)
    {
        Vector2 mousePos = Input.mousePosition;
        UI_TurnSlot closestSlot = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < slots.Count; ++i)
        {
            float curDistance = Vector2.Distance(mousePos, slots[i].transform.position);

            if (curDistance < closestDistance)
            {
                closestDistance = curDistance;
                closestSlot = slots[i];
            }
        }

        return closestSlot;
    }
}
