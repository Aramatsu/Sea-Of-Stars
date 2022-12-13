using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;

    //the struct that defines a sound
    [System.Serializable]
    public struct Sound
    {
        public string name;

        public GameObject SourceGameObject;

        [HideInInspector] public AudioSource Source;

        public AudioClip Clip;

        [Range(0f, 1f)] public float Volume;

        //constructor
        public Sound(string Name, GameObject SourceGameObject, AudioClip Clip, float Volume, AudioSource Source)
        {
            this.name = Name;
            this.Source = Source;
            this.SourceGameObject = SourceGameObject;
            this.Clip = Clip;
            this.Volume = Volume;
        }
    }

    public Sound[] Sounds;

    private void Awake()
    {

        audioManager = this;
        for(int i = 0; i < Sounds.Length; i++)
        {
            Sounds[i].Source = Sounds[i].SourceGameObject.AddComponent<AudioSource>();
            Sounds[i].Source.volume = Sounds[i].Volume;
            Sounds[i].Source.clip = Sounds[i].Clip;
        }
    }

    public void PlaySound(string SoundName, float volume)
    {
        AudioSource source = Array.Find<Sound>(Sounds, sound => sound.name == SoundName).Source;
        source.volume = volume;
        source.Play();

    }

    public void PlaySound(string SoundName)
    {
        AudioSource source = Array.Find<Sound>(Sounds, sound => sound.name == SoundName).Source;
        source.volume = 1;
        source.Play();
    }
}
