using TMPro;
using UnityEngine;

public class BlockIndicator : MonoBehaviour
{
    public static Color32 superColor = Color.green;
    public static Color32 activeColour = Color.white;
    public static Color32 inactiveColour = Color.gray;

    public SpriteRenderer[] pips = new SpriteRenderer[5];
    public TextMeshProUGUI hint;

    public AudioSource miscAudioSource;
    public AudioClip SuperReady;

    private void Start()
    {
        TurnOn(0);
    }

    // turns on $count pips. If count is equal to 5, then it sets them as superColor
    public void TurnOn(int count)
    {
        if (count == 5)
        {
            // turn them all superColor if super is ready
            for (int i = 0; i < pips.Length; i++)
            {
                pips[i].color = superColor;
            }
            hint.text = "Press Space!";

            miscAudioSource.clip = SuperReady;
            Util.AttemptPlay(miscAudioSource);
        }
        else
        {
            // turn on n
            for (int i = 0; i < count; i++)
            {
                pips[i].color = activeColour;
            }
            // turn off remaining
            for (int i = count; i < pips.Length; i++)
            {
                pips[i].color = inactiveColour;
            }
            hint.text = "";
        }
    }
}
