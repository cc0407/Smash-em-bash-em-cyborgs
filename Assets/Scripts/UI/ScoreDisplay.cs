using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public SegmentDisplay[] digitDisplays = new SegmentDisplay[3];

    public void Start()
    {
        ClearDisplay();
    }

    public void Display(int number)
    {
        foreach (SegmentDisplay s in digitDisplays)
        {
            s.Display(number);
            number /= 10;
        }
    }

    public void ClearDisplay()
    {
        foreach (SegmentDisplay s in digitDisplays)
        {
            s.Display(0);
        }
    }
}
