using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioClip[] musicClips;
    public AudioSource backgroundMusicSource;
    public AudioSource soundEffectSourcePrefab;
    public bool playMusicOnStart = true;

    private List<AudioSource> soundEffectSources = new List<AudioSource>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (playMusicOnStart)
        {
            PlayBackgroundMusic(0);
        }
    }

    public void PlayBackgroundMusic(int musicIndex)
    {
        if (musicIndex < 0 || musicIndex >= musicClips.Length)
        {
            Debug.LogWarning("Invalid music index!");
            return;
        }

        if (backgroundMusicSource.isPlaying)
            backgroundMusicSource.Stop();

        backgroundMusicSource.clip = musicClips[musicIndex];
        backgroundMusicSource.Play();
    }

    public void PlaySoundEffect(AudioClip soundClip)
    {
        AudioSource soundEffectSource = GetAvailableSoundEffectSource();
        if (soundEffectSource != null)
        {
            soundEffectSource.clip = soundClip;
            soundEffectSource.Play();
        }
    }

    private AudioSource GetAvailableSoundEffectSource()
    {
        foreach (AudioSource source in soundEffectSources)
        {
            if (!source.isPlaying)
                return source;
        }

        AudioSource newSource = Instantiate(soundEffectSourcePrefab, transform);
        soundEffectSources.Add(newSource);
        return newSource;
    }

    public void SetBGMVolume(float volume)
    {
        backgroundMusicSource.volume = volume;
    }
}

