using UnityEngine;

public class MatchTimer : MonoBehaviour
{
    public ScoreDisplay[] displays = new ScoreDisplay[4];
    private float time = 60f;
    private bool timerOn = false;
    // Start is called before the first frame update
    void Start()
    {
        UpdateAll(time);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            time -= Time.deltaTime;
            UpdateAll(time);
            if (time <= 0)
            {
                time = 0;
                timerOn = false;
                //Debug.LogWarning("MATCH OVER!");
                this.transform.parent.BroadcastMessage("EndMatch");
            }
        }
    }
    public void UpdateAll(float newTime)
    {

        foreach (ScoreDisplay sd in displays)
        {
            sd.Display((int)newTime);
        }
    }

    public void Activate()
    {
        timerOn = true;
    }
}
