using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_CassetteButton : MonoBehaviour
{
    [SerializeField] Transform graphicsHolder;
    [SerializeField] Image upImg;
    
    UI_CassetteControls cassette;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cassette = UI_CassetteControls.instance;
    }

    public void Release()
    {
        cassette = UI_CassetteControls.instance;
        
        DoKill();

        graphicsHolder.DOLocalMoveY(0f, cassette.releaseTime).SetEase(cassette.releaseCurve);
        upImg.DOFade(1f,  cassette.releaseTime).SetEase(cassette.releaseCurve);
    }

    public void Press()
    {
        DoKill();

        graphicsHolder.DOLocalMoveY(-cassette.buttonMoveDist, cassette.pressDownTime).SetEase(cassette.pressDownCurve);
        upImg.DOFade(0f,  cassette.pressDownTime).SetEase(cassette.pressDownCurve);
    }

    void DoKill()
    {
        graphicsHolder.DOKill(false);
        upImg.DOKill(false);
    }
}
