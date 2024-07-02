using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnArenaLoad : MonoBehaviour
{
    public GameObject EnemyWrapper;
    public PlayerController playerController;
    private AIController EnemyController;
    public MatchTimer timer;
    public SegmentDisplay countdown;

    public AudioSource musicSource;
    public AudioSource miscAudioSource;
    public AudioClip[] musicClips;
    public AudioClip countdownSound;
    public AudioClip matchStartSound;
    public AudioClip matchEndSound;

    void Awake()
    {
        EnemyController = Instantiate(ArenaSelector.chosenEnemy, EnemyWrapper.transform).transform.GetChild(0).GetComponent<AIController>();
    }

    IEnumerator Start()
    {
        yield return StartCoroutine(StartMatch());
    }

    public IEnumerator StartMatch()
    {
        // Begin countdown
        miscAudioSource.clip = countdownSound;
        for (int i = 3; i > 0; i--)
        {
            Util.AttemptPlay(miscAudioSource);
            countdown.Display(i);
            yield return new WaitForSeconds(1f);
        }

        // Play match start sound
        miscAudioSource.clip = matchStartSound;
        Util.AttemptPlay(miscAudioSource);

        Util.PlayRandomSound(musicClips, musicSource);

        // Remove countdown timer
        countdown.transform.gameObject.SetActive(false);

        // Start match
        EnemyController.Activate();
        playerController.Activate();
        timer.Activate();
    }

    public IEnumerator EndMatch()
    {
        // Play match end sound
        miscAudioSource.clip = matchEndSound;
        Util.AttemptPlay(miscAudioSource);

        // Deactivate all players
        EnemyController.Deactivate();
        playerController.Deactivate();

        // Add some visual breathing room
        yield return new WaitForSeconds(1f);

        // Determine the winner and switch scenes
        OnPodiumLoad.playerWon = playerController.status.score >= EnemyController.aiStatus.score;
        //Debug.Log(OnPodiumLoad.playerWon);
        SceneManager.LoadScene("Podium");
    }

}
