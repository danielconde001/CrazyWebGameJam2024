using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    private float TimeLeft = 0;
    private  bool TimerOn = false;

    public TextMeshProUGUI TimerTxt;
   
    public void StartTime(float timeLeft)
    {
        TimeLeft = timeLeft;
        TimerOn = true;
    }
    
    void Update()
    {
        if(TimerOn)
        {
            if(TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                UpdateTimer(TimeLeft);
            }
            else
            {
                TimeLeft = 0;
                TimerOn = false;
                GameManager.Instance().CheckLevelGoalStatus(TimeLeft);
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
