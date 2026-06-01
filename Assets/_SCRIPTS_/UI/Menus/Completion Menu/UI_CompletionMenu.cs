using System.Threading.Tasks;
using DG.Tweening;
using MapRooms;
using Saving;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(Canvas))]
public class UI_CompletionMenu : MonoBehaviour
{
    public static bool isOpen;
    public static UI_CompletionMenu instance;

    GraphicRaycaster graphicRaycaster;
    Canvas canvas;

    [SerializeField] RectTransform holder;
    [SerializeField] RectTransform bannerThing;
    [SerializeField] RectTransform menu;
    [SerializeField] Image darkness;

    [Space]

    [SerializeField] float openTime = 0.3f;
    [SerializeField] AnimationCurve holderXAnimCurve;
    [SerializeField] AnimationCurve holderYAnimCurve;
    [SerializeField] AnimationCurve bannerXAnimCurve;
    [SerializeField] AnimationCurve bannerYAnimCurve;

    [Space]

    [SerializeField] float menuOpenTime;
    [SerializeField] float menuOpenDelay = 0.3f;
    [SerializeField] AnimationCurve menuOpenAnimCurve;
    [SerializeField] RectTransform menuOffPos, menuOnPos;

    [Space]

    [SerializeField] float closeTime = 0.3f;
    [SerializeField] Transform closedMenuPosition;

    float openBuffer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Init();
    }

    void Init()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        canvas = GetComponent<Canvas>();

        instance = this;

        graphicRaycaster.enabled = false;
        isOpen = false;
        holder.localScale = Vector2.zero;
        canvas.enabled = false;
    }

    void Update()
    {
        openBuffer -= Time.unscaledTime;
    }

    public void Open()
    {
        if (openBuffer > 0f) return;

        isOpen = true;
        graphicRaycaster.enabled = true;
        canvas.enabled = true;

        holder.DOKill(false);
        bannerThing.DOKill(false);

        holder.localScale = Vector2.zero;
        holder.position = new Vector3(Screen.width/2f, Screen.height/2f);
        holder.DOScaleX(1f, openTime).SetEase(holderXAnimCurve);
        holder.DOScaleY(1f, openTime).SetEase(holderYAnimCurve);

        bannerThing.sizeDelta = new Vector2(0f, 230f);
        bannerThing.DOSizeDelta(new Vector2(800f, 230f), openTime).SetEase(bannerXAnimCurve);

        bannerThing.localScale = new Vector2(1f, 0f);
        bannerThing.DOScale(1f, openTime).SetEase(bannerYAnimCurve);

        menu.DOKill(false);
        menu.localPosition = menuOffPos.localPosition;
        menu.DOLocalMove(menuOnPos.localPosition, menuOpenTime).SetEase(menuOpenAnimCurve).SetDelay(menuOpenDelay);

        darkness.DOKill(false);
        darkness.DOFade(0.7f, openTime);

        MapRooms.Room room = MapRoomSystem.instance.GetCurrentRoom();

        if (room != null)
        {
            string roomID = room.roomUniqueID;

            Main.GetSaveManager().SetBool(roomID + "completed", true);
        }
    }

    public void Close()
    {
        isOpen = false;

        holder.DOKill(false);
        bannerThing.DOKill(false);
        menu.DOKill(false);
        darkness.DOKill(false);

        darkness.DOFade(0f, closeTime);

        holder.DOMove(closedMenuPosition.position, closeTime).SetEase(Ease.InQuad).OnComplete(() =>
        {
            graphicRaycaster.enabled = false;
            canvas.enabled = false;
        });

        openBuffer = 1f;
    }

    public void NextLevelBtn()
    {
        GoToNextLevel.NextLevel();

        Close();
    }
}
