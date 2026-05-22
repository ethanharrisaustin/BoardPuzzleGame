using UnityEngine;

public class UI_CassetteControls : MonoBehaviour
{
    public static UI_CassetteControls instance;

    [SerializeField] UI_CassetteButton playBtn;
    [SerializeField] UI_CassetteButton stopBtn;
    [SerializeField] UI_CassetteButton fastForwardBtn;

    [Space]

    public float buttonMoveDist;
    public AnimationCurve pressDownCurve;
    public float pressDownTime;
    public AnimationCurve releaseCurve;
    public float releaseTime;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Stop()
    {
        playBtn.Release();
        stopBtn.Press();
        if (fastForwardBtn != null) fastForwardBtn.Release();
    }

    public void Play()
    {
        playBtn.Press();
        stopBtn.Release();
        if (fastForwardBtn != null) fastForwardBtn.Release();
    }

    public void FastFoward()
    {
        playBtn.Release();
        stopBtn.Release();
        if (fastForwardBtn != null) fastForwardBtn.Press();
    }
}
