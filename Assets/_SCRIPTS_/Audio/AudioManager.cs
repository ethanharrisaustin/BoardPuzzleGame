using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine.Rendering;
using FMOD;

public class AudioManager : MonoBehaviour
{
    //Clean-up between scenes
    private List<EventInstance> eventInstances;

    //EventInstance is an FMOD type that handles audio events
    private EventInstance musicEventInstance;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            UnityEngine.Debug.LogError("More than one Audio Manager Found"); // We only want one Audio Manager in the scene!
        }
         
        instance = this;

        eventInstances = new List<EventInstance>(); // Initialise list for cleanup

    }
    private void Start()
    {
        InitialiseMusic(FMODEvents.instance.music); //Play music
    }

    public EventInstance CreateFMODEventInstance(EventReference eventReference) // Gets any audio event instance
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public void SetRank(Rank currentRank)
    {
        musicEventInstance.setParameterByName("Rank", (float)currentRank); // Controls music intensity
    }
    private void InitialiseMusic(EventReference music) //Gets music audio event
    {
        musicEventInstance = CreateFMODEventInstance(music);
        musicEventInstance.start();
    }

    //Allows one-time sound effects (can be adapted for enemy sounds)
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    //Stop all audio events and destroy them (not sure about memory efficiency)
    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp(); 
    }

    // Called from MusicRhythmTimer.cs in Update() to calculate MusicDelta
    public static float MusicTime()
    {
        instance.musicEventInstance.getChannelGroup(out ChannelGroup channelGroup);

        channelGroup.getDSPClock(out ulong clock, out _);
        RuntimeManager.CoreSystem.getSoftwareFormat(out int sampleRate, out _, out _);

        return clock / (float)sampleRate;
    }

    
}
