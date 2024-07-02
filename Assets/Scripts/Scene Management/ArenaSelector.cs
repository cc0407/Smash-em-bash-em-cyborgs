using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaSelector : MonoBehaviour
{
    public static GameObject chosenEnemy { get; set; }
    public GameObject EnemyDisplay;
    private GameObject beingDisplayed;
    public GameObject StartButton;
    public TextMeshProUGUI FlavourTextDisplay;
    public CameraPan CameraPanScript;
    public Canvas FighterSelect;
    public Canvas MainMenu;
    public Canvas Info;
    public Canvas Settings;
    public ArenaAudioSelector music;

    // This keeps track of current transition callback for camera pan.
    private Coroutine currentAwait;

    public void Start()
    {
        ActivateMainMenu(); // Set main menu canvas as active
    }

    // Selects an opponent to fight, and displays their information on the UI
    public void ChooseEnemy(GameObject enemy)
    {
        music.UIClick();
        // Keep a record of chosen enemy
        chosenEnemy = enemy;

        // Destroy currently displayed bot if present
        if (beingDisplayed != null)
            Object.Destroy(beingDisplayed);

        beingDisplayed = Instantiate(enemy, EnemyDisplay.transform);

        StartButton.SetActive(true);
        FlavourTextDisplay.SetText(beingDisplayed.transform.GetChild(0).GetComponent<AIData>().FlavourText);
    }

    // This is a helper function for NavigateToArena. Is used as a callback
    public void StartWithChosenEnemy()
    {
        if (chosenEnemy != null)
        {
            CameraPanScript.SetTarget(CameraPan.cameraPositions.ARENASTART);
            SceneManager.LoadScene("Arena");
        }
    }

    // Use this one. Will pan camera and then switch scenes
    public void NavigateToArena()
    {
        if (chosenEnemy != null)
        {
            CameraPan.cameraPositions target = CameraPan.cameraPositions.ARENASTART;
            music.UIClick(); // make a UI click sound
            MenuTransition(); // temporarily shut off canvases

            CameraPanScript.SetTarget(target);
            if(currentAwait != null)
                StopCoroutine(currentAwait);
            currentAwait = StartCoroutine(CameraPanScript.WaitForPosition(StartWithChosenEnemy, target)); // wait for camera to be in position, then activate
        }
    }

    // Use this one. Will pan camera and then switch scenes
    public void NavigateToFighterSelect()
    {
        CameraPan.cameraPositions target = CameraPan.cameraPositions.FIGHTERSELECT;
        music.UIClick(); // make a UI click sound
        MenuTransition(); // temporarily shut off canvases
        music.Play((int)target); // Switch music tracks to next song

        CameraPanScript.SetTarget(target); // set fighter select as camera panning target
        if (currentAwait != null)
            StopCoroutine(currentAwait);
       currentAwait = StartCoroutine(CameraPanScript.WaitForPosition(ActivateFighterSelect, target)); // wait for camera to be in position, then activate
    }

        // This is a helper function for NavigateToFighterSelect. Is used as a callback
    public void ActivateFighterSelect()
    {
        MenuTransition();
        FighterSelect.gameObject.SetActive(true);
    }

    // Use this one. Will pan camera and then switch scenes
    public void NavigateToMainMenu(bool changeMusic)
    {
        CameraPan.cameraPositions target = CameraPan.cameraPositions.MAINMENU;
        music.UIClick(); // make a UI click sound
        MenuTransition(); // temporarily shut off canvases
        if (changeMusic)
        {
            music.Play((int)target); // Switch music tracks to next song
        }


            CameraPanScript.SetTarget(target); // set fighter select as camera panning target
        if (currentAwait != null)
            StopCoroutine(currentAwait);
        currentAwait = StartCoroutine(CameraPanScript.WaitForPosition(ActivateMainMenu, target)); // wait for camera to be in position, then activate
    }

    // This is a helper function for NavigateToMainMenu. Is used as a callback
    public void ActivateMainMenu()
    {
        MenuTransition();
        MainMenu.gameObject.SetActive(true);
    }

    // Use this one. Will pan camera and then switch scenes
    public void NavigateToInfo()
    {
        CameraPan.cameraPositions target = CameraPan.cameraPositions.INFO;
        music.UIClick(); // make a UI click sound
        MenuTransition(); // temporarily shut off canvases

        CameraPanScript.SetTarget(target); // set fighter select as camera panning target
        if (currentAwait != null)
            StopCoroutine(currentAwait);
        currentAwait = StartCoroutine(CameraPanScript.WaitForPosition(ActivateInfo, target)); // wait for camera to be in position, then activate
    }

    // This is a helper function for NavigateToInfo. Is used as a callback
    public void ActivateInfo()
    {
        MenuTransition();
        Info.gameObject.SetActive(true);
    }

    // Use this one. Will pan camera and then switch scenes
    public void NavigateToSettings()
    {
        CameraPan.cameraPositions target = CameraPan.cameraPositions.SETTINGS;
        music.UIClick(); // make a UI click sound
        MenuTransition(); // temporarily shut off canvases

        CameraPanScript.SetTarget(target); // set fighter select as camera panning target
        if (currentAwait != null)
            StopCoroutine(currentAwait);
        currentAwait = StartCoroutine(CameraPanScript.WaitForPosition(ActivateSettings, target)); // wait for camera to be in position, then activate
    }

    // This is a helper function for NavigateToSettings. Is used as a callback
    public void ActivateSettings()
    {
        MenuTransition();
        Settings.gameObject.SetActive(true);
    }

    // Shuts off all canvases
    public void MenuTransition()
    {
        MainMenu.gameObject.SetActive(false);
        FighterSelect.gameObject.SetActive(false);
        Info.gameObject.SetActive(false);
        Settings.gameObject.SetActive(false);
    }
}
