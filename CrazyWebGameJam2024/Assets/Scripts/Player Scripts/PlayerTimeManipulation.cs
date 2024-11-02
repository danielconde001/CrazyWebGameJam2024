using UnityEngine;
using DG.Tweening;

public class PlayerTimeManipulation : PlayerControl
{
    [SerializeField] private float defaultTimeSlowDuration;
    [SerializeField] private float timeSlowTransitionDuration;
    [SerializeField] private float slowTimeScale;
    [SerializeField] private float normalTimeScale;

    private bool isTimeSlowed = false;
    private float timeSlowTimer = 0.0f;

    public void SlowTime()
    {
        SlowTime(defaultTimeSlowDuration);
    }

    public void SlowTime(float timeSlowDuration)
    {
        if(timeSlowDuration > 0.0f)
        {
            timeSlowTimer = timeSlowDuration;
            DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, slowTimeScale, timeSlowTransitionDuration).SetUpdate(UpdateType.Normal, true);
            isTimeSlowed = true;
        }
    }

    public void NormalizeTime()
    {
        if(isTimeSlowed == true)
        {
            DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, normalTimeScale, timeSlowTransitionDuration).SetUpdate(UpdateType.Normal, true);
            isTimeSlowed = false;
        }
    }

    private void Update()
    {
        if(canControl == false)
        {
            return;
        }

        TimeSlowTimer();
    }

    private void TimeSlowTimer()
    {
        if(isTimeSlowed == true)
        {
            timeSlowTimer -= Time.unscaledDeltaTime;

            if(timeSlowTimer <= 0.0f)
            {
                NormalizeTime();
            }
        }
    }
}
