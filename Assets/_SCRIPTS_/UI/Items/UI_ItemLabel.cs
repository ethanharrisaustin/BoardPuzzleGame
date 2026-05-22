using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(TMP_Text))]
public class UI_ItemLabel : MonoBehaviour
{
    public static UI_ItemLabel instance;

    [SerializeField] float appearSpeed = 100f;
    [SerializeField] float disappearTime = 10f;

    CanvasGroup canvasGroup;
    TMP_Text labelTxt;

    float labelAlpha = 0f;

    void Awake()
    {
        instance = this;

        canvasGroup = GetComponent<CanvasGroup>();
        labelTxt = GetComponent<TMP_Text>();

        canvasGroup.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        labelAlpha -= Time.deltaTime * disappearTime;

        if (labelAlpha < 0f) labelAlpha = 0f;

        canvasGroup.alpha = labelAlpha;
    }

    public void ShowLabel(string label)
    {
        labelTxt.text = label;
        labelAlpha += Time.deltaTime * appearSpeed;

        if (labelAlpha > 1.4f) labelAlpha = 1.4f;
    }


}
