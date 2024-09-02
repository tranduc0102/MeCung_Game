using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    public AudioSource audioSource;
    public AudioSource audioSFX;

    public AudioClip musicGame;
    public AudioClip musicClickButton;
    public AudioClip musicClickGround;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip audio)
    {
        audioSFX.clip = audio;
        audioSFX.Play();
    }

    public void PlayAudioSource(AudioClip audio)
    {
        audioSource.Stop();
        audioSource.clip = audio;
        audioSource.volume = 1f;
        audioSource.Play();
        audioSource.loop = true;
    }
}
