using System.Collections.Generic;
using UnityEngine;

public class BatteryIndicator : MonoBehaviour
{
    protected List<GameObject> batteryPipList; // The pip gameobjects
    protected float[] batterySplits = new float[5] // The percentages that denote each pip
        {
            80,
            60,
            40,
            20,
            5,
        };
    // Start is called before the first frame update
    void Start()
    {
        batteryPipList = new List<GameObject>();
        for (int i = 0; i < batterySplits.Length; i++)
        {
            batteryPipList.Add(this.transform.GetChild(i).gameObject);
        }

        // set it to green on start
        UpdateIndicatorColour(false);
    }

    // Assumes 10f max health, conditionally displays battery pips depending on $health provided
    // Gets broadcasted to by AIStatus & Playerstatus
    protected virtual void UpdateIndicatorDisplay(float health)
    {
        float per = health * 10;
        int end = batterySplits.Length;

        for (int i = 0; i < end; i++)
        {
            if (per > batterySplits[i]) { batteryPipList[i].SetActive(true); }
            else { batteryPipList[i].SetActive(false); }
        }
    }


    // Gets broadcasted to by AIStatus & PlayerStatus
    protected virtual void UpdateIndicatorColour(bool isLowHealth)
    {
        // Change the colour based on low health percent
        if (isLowHealth)
            ChangeColour(Color.red);
        else
            ChangeColour(Color.green);
    }

    protected void ChangeColour(Color c)
    {
        foreach (Renderer r in transform.GetComponentsInChildren<Renderer>())
        {
            r.material.SetColor("_Color", c);
        }
    }
}
