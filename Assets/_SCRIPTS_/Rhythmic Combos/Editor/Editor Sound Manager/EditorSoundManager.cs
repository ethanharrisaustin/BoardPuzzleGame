using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


public class EditorSoundManager 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateMetronomeSounds()
    {
        GameObject obj = EditorCacheGO();

        if (obj.transform.childCount == 0)
        {
            GameObject child1 = new GameObject("1");
            GameObject child2 = new GameObject("2");

            child1.transform.parent = obj.transform;
            child2.transform.parent = obj.transform;

            AudioSource click1 = child1.AddComponent<AudioSource>();
            AudioSource click2 = child2.AddComponent<AudioSource>();

            click2.pitch = 0.8f;
            click2.volume *= 0.7f;

            click1.resource = Resources.Load("Rhythmic Combos/Metronome Click") as AudioClip;
            click2.resource = Resources.Load("Rhythmic Combos/Metronome Click") as AudioClip;

            click1.playOnAwake = false; click2.playOnAwake = false;
        }

        PlaySomeSounds(obj.transform.GetChild(0).GetComponent<AudioSource>(), obj.transform.GetChild(1).GetComponent<AudioSource>());
    }

    async void PlaySomeSounds(AudioSource click1, AudioSource click2)
    {
        for (int i = 0; i < 8; ++i)
        {


            bool isBar = i % 4 == 0;

            if (isBar) click1.Play(); else click2.Play();
            

             await Task.Delay(500);
        }
    }

    GameObject EditorCacheGO()
    {
        GameObject obj = GameObject.Find("EDITOR CACHE (Please Delete)");

        if (obj != null && obj.transform.childCount != 2)
        {
            EditorWindow.DestroyImmediate(obj);
        }

        if (obj == null) obj = new GameObject("EDITOR CACHE (Please Delete)");

        return obj;
    }
}
