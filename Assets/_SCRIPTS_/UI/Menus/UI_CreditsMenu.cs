using UnityEngine;
using UnityEngine.UI;

public class UI_CreditsMenu : MonoBehaviour
{

    [SerializeField] float scrollSpeed;
    [SerializeField] Scrollbar scrollbar;

    float setToZeroTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (setToZeroTimer > 0f)
        {
            scrollbar.value = 1f;
            setToZeroTimer -= Time.unscaledDeltaTime;
            return;
        }

        scrollbar.value -= Time.unscaledDeltaTime * scrollSpeed;
    }

    void OnEnable()
    {
        setToZeroTimer = 0.5f;
    }
}
