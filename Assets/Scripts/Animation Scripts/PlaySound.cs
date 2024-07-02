using UnityEngine;

public class PlaySound : StateMachineBehaviour
{

    public AudioClip[] clipEnter;
    public AudioClip[] clipExit;
    private AudioSource src;

    public void Play(AudioClip c, Animator a)
    {
        if (c != null)
        {
            src = a.gameObject.GetComponent<AudioSource>();
            if (src != null)
            {
                src.clip = c;
                Util.AttemptPlay(src);
            }
            else
            {
                Debug.LogError("Could not find audioSource attached to: " + a.gameObject.name + ", when attempting to play: " + c.name);
            }
        }

    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Play(Util.GetRandomSound(clipEnter), animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Play(Util.GetRandomSound(clipExit), animator);
    }
}
