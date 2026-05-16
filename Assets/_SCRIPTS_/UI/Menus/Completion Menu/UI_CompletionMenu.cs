using System.Threading.Tasks;
using DG.Tweening;
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

    public void Open()
    {
        isOpen = true;
        graphicRaycaster.enabled = true;
        canvas.enabled = true;

        holder.DOKill(false);
        bannerThing.DOKill(false);

        holder.localScale = Vector2.zero;
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
    }
}
