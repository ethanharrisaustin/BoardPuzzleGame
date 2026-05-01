using UnityEngine;

public class RB_BeatSpawner : MonoBehaviour
{
    public ObjectPool objectPool;
    public BasicMetronomeObject basicMetronomeObject;
    public float _pixelsPerSecond = 200f;

    public AudioSource music;
    [SerializeField]public bool debugBarEnabled = false;

    public float counter = 0f;

    int currentBeat = 0;

    public static float pixelsPerSecond { get { return instance._pixelsPerSecond; } }

    public static RB_BeatSpawner instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (MusicRhythmTimer.MusicDelta() < 0) return;

        _Spawner();
    }

    void _Spawner()
    {
        counter += MusicRhythmTimer.MusicDelta();

        if (counter >= basicMetronomeObject.SecondsBetweenBeats())
        {
            float remainder = (float)basicMetronomeObject.SecondsBetweenBeats() - counter;

            RB_Beat beat = objectPool.SpawnObject().GetComponent<RB_Beat>();

            beat.SpawnBeat(currentBeat, basicMetronomeObject);

            beat.MoveBeat(-basicMetronomeObject.beatsInBar * (1f + remainder));

            counter = -remainder;

            currentBeat++;
        }
    }
    public float CalculateDebugDistanceBetweenBeats()
    {
        float currentXPos;
        float furtherestXPos=999999;
        RB_Beat furtherestXPosBeat = null;
        
        objectPool.LoopThroughActiveObjects((RB_Beat beat) =>
        {
            currentXPos = beat.transform.localPosition.x;
            if(currentXPos < furtherestXPos)
            {
                furtherestXPos = currentXPos;
                furtherestXPosBeat = beat;
            }
        });

        furtherestXPos = 999999;
        RB_Beat secondfurthestXPosBeat = null;
        objectPool.LoopThroughActiveObjects((RB_Beat beat) =>
        {
            if (beat != furtherestXPosBeat)
            {
                currentXPos = beat.transform.localPosition.x;
                if (currentXPos < furtherestXPos)
                {
                    furtherestXPos = currentXPos;
                    secondfurthestXPosBeat = beat;
                }
            }
        });

        if (furtherestXPosBeat == null || secondfurthestXPosBeat == null) return 0f;

        return Mathf.Abs(furtherestXPosBeat.transform.localPosition.x - secondfurthestXPosBeat.transform.localPosition.x);

    }
}