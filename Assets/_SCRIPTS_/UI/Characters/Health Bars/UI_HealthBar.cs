using MapRooms;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    public ObjectWithHealthGO objectWithHealthGO;
    Image fill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fill = transform.GetChild(0).GetComponent<Image>();
    }

    float refFillAmount = 1f;
    // Update is called once per frame
    void LateUpdate()
    {
        if (NeedsToDestroy())
        {
            gameObject.SetActive(false);
            return;
        }

        PositionHealthBar();

        ShowFill();
    }

    void PositionHealthBar()
    {
        if (objectWithHealthGO == null) return;
         
        transform.position = Camera.main.WorldToScreenPoint(objectWithHealthGO.transform.position) + Vector3.up * objectWithHealthGO.HealthBarOffsetY();
    }

    void ShowFill()
    {
        float targetFillAmount = HealthPercentage();
        float currentFillAmount = fill.fillAmount;
        fill.fillAmount = Mathf.SmoothDamp(currentFillAmount, targetFillAmount, ref refFillAmount, 0.2f);
    }

    float HealthPercentage()
    {
        if (objectWithHealthGO == null)
        {
            return 0f;
        }

        return objectWithHealthGO.HealthAsPercentage() * 0.01f;
    }

    bool NeedsToDestroy()
    {
        return fill.fillAmount < 0.01f;
    }
}
