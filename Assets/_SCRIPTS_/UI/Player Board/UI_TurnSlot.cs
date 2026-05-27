using System.Collections.Generic;
using System.Threading.Tasks;
using Cardboard;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoveItMoveIt
{
    public class UI_TurnSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IItem
    {
        #region Variables

        static List<UI_TurnSlot> turnSlots = new List<UI_TurnSlot>();

        [SerializeField] Color backgroundOffColour, backgroundOnColour;

        Image background { get { return childImages[0]; } } 
        [SerializeField] Image[] childImages;

        UI_Card_Base currentCard;

        bool hasCard { get { return currentCard != null; } }

        public static UI_TurnSlot hoveredSlot = null;
        public static UI_TurnSlot cardRectHoveredSlot = null;

        bool mouseWithCardOver = false;
        bool mouseOver = false;
        bool mouseDown = false;

        Vector2 mouseDownPosition;
        Vector2 mouseOffset;

        UI_PlayerTurnBoard playerTurnBoard;

        [SerializeField] bool interactable = true;

        [SerializeField] Color highlightColour;
        [SerializeField] Color unhighlightColour;
        [SerializeField] Color impossibleMoveColour;

        public string unique_id 
        { 
            get 
            { 
                if (currentCard != null)
                    return  currentCard.unique_id; 
                else 
                    return "";
            } 
        }

        #endregion

        #region Monobehaviour Functions

        void Awake()
        {
            playerTurnBoard = GetComponentInParent<UI_PlayerTurnBoard>();
        }

        void OnEnable()
        {
            if (interactable) turnSlots.Add(this);
        }

        void Update()
        {
            mouseWithCardHoverTimer -= Time.deltaTime;
        }

        #endregion

        #region IPointer Events

        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseOver = true;

            MouseEnterWithCard();
            MouseEnterWithoutCard();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            mouseOver = false;

            MouseExitWithCard();
            MouseExitWithoutCard();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            MouseDownWithoutCard();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            MouseUpWithoutCard();

            UI_DraggedItem.Get().MouseUp();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            MouseMove();
        }

        #endregion

        #region Dragging in Cards

        float mouseWithCardHoverTimer = 0f;
        void MouseEnterWithCard()
        {
            if (!ValidCardDragged()) return;

            Highlight();

            hoveredSlot = this;

            mouseWithCardOver = true;

            PlayHoverSFX();
        }

        void MouseExitWithCard()
        {
            Unhighlight();

            if (hoveredSlot == this) hoveredSlot = null;

            mouseWithCardOver = false;
        }

        public void CardRectEnter()
        {
            Highlight();

            cardRectHoveredSlot = this;

            PlayHoverSFX();
        }

        public static void CardRectExit()
        {
            cardRectHoveredSlot = null;

            for (int i = 0; i < turnSlots.Count; ++i)
            {
                if (NullOrDisabled(turnSlots[i])) continue;

                if (turnSlots[i].mouseWithCardOver) continue;

                turnSlots[i].Unhighlight();
            }       
        }

        void PlayHoverSFX()
        {
            if (mouseWithCardHoverTimer < 0f) AudioManager.Play("UI Pluck 2");

            mouseWithCardHoverTimer = 0.1f;
        }

        bool ValidCardDragged()
        {
            return UI_DraggedItem.IsDraggingItem<UI_Card_Base>();
        }

        public void AddCard(UI_Card_Base card, bool withNotify = true)
        {
            HideChildImages();

            Image[] imagesToCopy = card.GetImages();

            for (int i = 1; i < Mathf.Min(childImages.Length, imagesToCopy.Length + 1); ++i)
            {
                childImages[i].enabled = true;

                CardboardExtras.MatchImageAToB(childImages[i], imagesToCopy[i - 1]);
            }

            currentCard = card;

            MouseUp();

            if (withNotify) playerTurnBoard.OnTurnsChange();
        }

        static List<UI_TurnSlot> cachedOverlappingSlots = new List<UI_TurnSlot>();
        public static List<UI_TurnSlot> OverlappingSlots(UI_DraggedItem draggedItem)
        {
            cachedOverlappingSlots.Clear();

            RectTransform rectTransform = draggedItem.GetImages()[0].rectTransform;

            for (int i = 0; i < turnSlots.Count; ++i)
            {
                if (NullOrDisabled(turnSlots[i])) continue;

                RectTransform slotRect = turnSlots[i].BackgroundImage().rectTransform;

                if (RectExtensions.IsOverlapping(rectTransform, slotRect))
                {
                    cachedOverlappingSlots.Add(turnSlots[i]);
                }
            }

            return cachedOverlappingSlots;
        }

        #endregion

        #region Highlighting

        public void Highlight()
        {
            SetColour(backgroundOnColour, highlightColour);
        }

        public void Unhighlight()
        {
            SetColour(backgroundOffColour, GetColour(unhighlightColour, UI_Item_Base.normalBrightness));
        }

        void SetColour(Color backgroundColour, Color imagesColour)
        {
            background.color = backgroundColour;

            if (!hasCard) 
            {
                HideChildImages();
                return;
            }

            for (int i = 1; i < Mathf.Min(childImages.Length, currentCard.originalColour.Length); ++i)
            {
                if (childImages[i] == background) continue;
                
                childImages[i].color = currentCard.originalColour[i - 1] * imagesColour;
            }
        }

        public async void ShowImpossibleMove()
        {
            for (int i = 0; i < 3; ++i)
            {
                SetColour(backgroundOnColour, impossibleMoveColour);

                await Task.Delay(200);

                SetColour(backgroundOffColour, GetColour(unhighlightColour, 0.5f));

                await Task.Delay(200);
            }
        }

        void PressedDown()
        {
            background.color = backgroundOffColour;

            if (!hasCard) 
            {
                HideChildImages();
                return;
            }

            for (int i = 1; i < Mathf.Min(childImages.Length, currentCard.originalColour.Length); ++i)
            {
                if (childImages[i] == background) continue;
                
                childImages[i].color = GetColour(currentCard.originalColour[i - 1], UI_Item_Base.pressedBrightness);
            }
        }

        Color GetColour(Color original, float multiplier)
        {
            return new Color(original.r * multiplier, original.g * multiplier, original.b * multiplier);
        }

        #endregion

        #region Dragging Cards Off

        void MouseEnterWithoutCard()
        {
            if (!hasCard) return;

            if (UI_DraggedItem.IsDraggingItem()) return;

            Highlight();
        }

        void MouseExitWithoutCard()
        {
            if (!hasCard) return;

            if (UI_DraggedItem.IsDraggingItem()) return;

            Unhighlight();
        }

        void MouseDownWithoutCard()
        {
            if (!hasCard) return;

            if (UI_DraggedItem.IsDraggingItem()) return;

            PressedDown();

            mouseDown = true;

            mouseDownPosition = Input.mousePosition;
            mouseOffset = mouseDownPosition - (Vector2)transform.position;
        }

        void MouseUpWithoutCard()
        {
            if (!hasCard) return;

            if (UI_DraggedItem.IsDraggingItem()) return;

            MouseUp();

            if (currentCard != null)
                UI_ItemMoveTo.Get().SetUp(currentCard, transform.position);

            SetSlotAsEmpty();

            mouseDown = false;
        }

        void MouseUp()
        {
            if (mouseOver)
            {
                Highlight();
            }
            else
            {
                Unhighlight();
            }
        }

        void MouseMove()
        {
            if (!hasCard || !mouseDown || UI_DraggedItem.IsDraggingItem()) return;

            float distanceDragged = Vector2.Distance(Input.mousePosition, mouseDownPosition);

            if (distanceDragged > 2)
            {
                StartDraggingCard();

                mouseDown = false;
            }
        }

        void StartDraggingCard()
        {
            UI_DraggedItem.Get().SetUpDrag(currentCard, mouseOffset);

            SetSlotAsEmpty();
        }

        void SetSlotAsEmpty()
        {
            currentCard = null;
            HideChildImages();

            playerTurnBoard.OnTurnsChange();
        }

        public void SetSlotAsEmptyWithoutNotify()
        {
            currentCard = null;
            HideChildImages();
        }

        public void ShowCard(string unique_id)
        {
            UI_Card_Base uI_Card_Base = CardboardItems.GetCardItem(unique_id);

            if (uI_Card_Base == null) return;

            AddCard(uI_Card_Base, false);
        }

        #endregion

        #region General

        public Image BackgroundImage()
        {
            return background;
        }

        public Image[] GetImages()
        {
            return childImages;
        }

    
        public void HideChildImages()
        {
            for (int i = 0; i < childImages.Length; ++i)
            {
                if (childImages[i] == background) continue;
                
                childImages[i].enabled = false;
            }
        }

        static bool NullOrDisabled(UI_TurnSlot uI_TurnSlot)
        {
            if (uI_TurnSlot == null) return true;
            if (uI_TurnSlot.enabled == false) return true;
            if (uI_TurnSlot.gameObject.activeInHierarchy == false) return true;

            return false;
        }

        public string CardsUniqueID()
        {
            if (currentCard == null) return "";

            return currentCard.unique_id;
        }

        #endregion
    }
}