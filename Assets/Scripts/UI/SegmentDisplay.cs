using UnityEngine;

public class SegmentDisplay : MonoBehaviour
{
    public Color32 activeColour = Color.red;
    public Color32 inactiveColour = Color.black;

    public SpriteRenderer[] segments = new SpriteRenderer[7];

    public void Display(int number)
    {
        var digit = number % 10;
        if (digit < 0) digit *= -1;

        for (int i = 0; i < 7; i++)
        {
            if (Util.DigitDisplay[digit, i])
            {
                segments[i].color = activeColour;
            }
            else
            {
                segments[i].color = inactiveColour;
            }
        }
    }
}
