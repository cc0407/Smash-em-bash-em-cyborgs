using UnityEngine;

public class ArenaAudioSelector : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource miscSource;

    public AudioClip[] musicClips;
    public AudioClip uiClickSound;

    private int current = 0;

    public void Start()
    {
        Play(current);
    }
    public void Play(int index)
    {
        if (index >= 0 && index < musicClips.Length)
        {
            musicSource.clip = musicClips[index];
            Util.AttemptPlay(musicSource);
        }
    }

    public void UIClick()
    {
        miscSource.clip = uiClickSound;
        Util.AttemptPlay(miscSource);
    }
}
