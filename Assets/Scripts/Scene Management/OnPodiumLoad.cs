using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnPodiumLoad : MonoBehaviour
{

    public static bool playerWon;
    public AnimatorController win;
    public AnimatorController loss;
    public Animator playerAnimator;
    public GameObject EnemyWrapper;
    public TextMeshProUGUI resultDisplay;
    private GameObject enemy;

    public AudioSource musicSource;
    public AudioClip winMusic;
    public AudioClip lossMusic;

    // Start is called before the first frame update
    void Start()
    {
        // Add enemy to the game, and remove all the scripts that may introduce buggy behaviour
        enemy = Instantiate(ArenaSelector.chosenEnemy, EnemyWrapper.transform).transform.GetChild(0).gameObject;
        Destroy(enemy.GetComponent<AIStatus>());
        Destroy(enemy.GetComponent<AIAnimationController>());
        Destroy(enemy.GetComponent<AIController>());

        // Manually set the animator controller, each should contain an automatic animation
        if (playerWon)
        {
            playerAnimator.runtimeAnimatorController = win;
            enemy.GetComponent<Animator>().runtimeAnimatorController = loss;
            resultDisplay.text = "You won!";
            resultDisplay.color = Color.green;
            musicSource.clip = winMusic;
        }
        else
        {
            playerAnimator.runtimeAnimatorController = loss;
            enemy.GetComponent<Animator>().runtimeAnimatorController = win;
            resultDisplay.text = "You lost...";
            resultDisplay.color = Color.red;
            musicSource.clip = lossMusic;
        }

        musicSource.Play();
    }

    public void ReturnToMenu()
    {
        playerWon = false;
        ArenaSelector.chosenEnemy = null;
        SceneManager.LoadScene("MainMenu");
    }
}
