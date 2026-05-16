using UnityEngine;
//using FMOD;

public class MusicRhythmTimer : MonoBehaviour
{
    public BasicMetronomeObject basicMetronomeObject;

    public static MusicRhythmTimer instance;

    int bar = 1;
    int beat = 1;

    float beatTimer = 0f;
    float barTimer = 0f;

    float musicTimeLastFrame = 0;

    float musicDelta = 0f;

    bool increasedBeat = false;
    bool offBeatIncreased = false;
    bool offBeatWaiting = false;

    [SerializeField] float rhythmAllowance = 4f;
    void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        CalculateMusicDelta();

        BeatsAndBars();

        OffBeats();
    }

    void CalculateMusicDelta()
    {
        float time = 0;//AudioManager.MusicTime();

        musicDelta = musicTimeLastFrame > time ? 0 : time - musicTimeLastFrame;

        musicTimeLastFrame = time;
    }

    void BeatsAndBars()
    {
        increasedBeat = false;

        beatTimer += musicDelta;
        barTimer += musicDelta;

        if (beatTimer >= basicMetronomeObject.SecondsBetweenBeats())
        {
            double remainder = beatTimer - basicMetronomeObject.SecondsBetweenBeats();

            beat++;

            increasedBeat = true;

            if (beat > basicMetronomeObject.beatsInBar)
            {
                beat = 1;
                bar++;
                barTimer = (float)remainder;
            }

            beatTimer = (float)remainder;
        }
    }

    void OffBeats()
    {
        offBeatIncreased = false;

        if (offBeatWaiting)
        {
            if (increasedBeat)
            {
                offBeatWaiting = false;
            }
            return;    
        }

        if (beatTimer >= basicMetronomeObject.SecondsBetweenBeats() * 0.5f)
        {
            offBeatIncreased = true;

            offBeatWaiting = true;
        }
    }


    public bool OnBar()
    {
        return barTimer < Allowence() || barTimer > basicMetronomeObject.SecondsInABar() - Allowence();
    }

    public bool OnBeat(int beat = -1)
    {
        if (beat < 1)
        {
            return beatTimer < Allowence() || beatTimer > basicMetronomeObject.SecondsBetweenBeats() - Allowence();
        }

        if (this.beat == beat)
        {
            return beatTimer < Allowence();
        }
        else if (this.beat + 1 == beat)
        {
            return beatTimer > basicMetronomeObject.SecondsBetweenBeats() - Allowence();
        }

        return false;
    }

    /// <summary>
    /// This gives a percenage: 1 is bang on, 0 is just within allowence. Less than 0 means it was too off beat to be allowed!
    /// </summary>
    /// <returns></returns>
    public float Accuracy()
    {
        return  ((Allowence() - DistanceFromBeat()) / Allowence()*-1);
    }

    public float DistanceFromBeat()
    {
        float previousBeat = PreviousBeat();
        float nextBeat = NextBeat();

        // We are closer to previous beat 
        if (ClosestToBeatA(previousBeat, nextBeat))
        {
            UnityEngine.Debug.Log("We are closest to the previous beat");
            return Mathf.Abs(previousBeat);
        }
        UnityEngine.Debug.Log("We are closest to the next beat");
        return nextBeat;
    }

    public static float MusicDelta()
    {
        return instance.musicDelta;
    }
    
    public static double SecondsBetweenBeats()
    {
        if (instance == null) return 0f;
        return instance.basicMetronomeObject.SecondsBetweenBeats();
    }

    public static double SecondsBetweenBars()
    {
        return instance.basicMetronomeObject.SecondsBetweenBeats() * instance.basicMetronomeObject.beatsInBar;
    }

    public static bool BeatIncreased()
    {
        return instance.increasedBeat;
    }

    /// <summary>
    /// When the music has increased by half a beat's worth of seconds.
    /// (Use OffBeatIncreased() to make object move on every off beat.)
    /// </summary>
    public static bool HalfBeatIncreased()
    {
        return instance.increasedBeat || instance.offBeatIncreased;
    }

    public static bool OffBeatIncreased()
    {
        return instance.offBeatIncreased;
    }

    float PreviousBeat()
    {
        return -beatTimer;
    }

    float NextBeat()
    {
        return (float)basicMetronomeObject.SecondsBetweenBeats() - beatTimer;
    }

    bool ClosestToBeatA(float a, float b)
    {
        return Mathf.Abs(a) > b;
    }
    
    public float Allowence()
    {
        return (float)basicMetronomeObject.SecondsBetweenBeats() * rhythmAllowance;
    }
}
