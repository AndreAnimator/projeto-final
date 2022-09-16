using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start (){
        Play("Theme");
    }

    public void Play(String name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null){
            Debug.Log("nao achou");
            return;
        }
        s.source.Play();
    }

    public void Pause(String name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null){
            Debug.Log("nao achou");
            return;
        }
        s.source.Pause();
    }
}
