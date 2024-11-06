using System;
using UnityEngine;
using DG.Tweening;

public class TimeManipulation : PlayerControl
{
    [SerializeField] private float defaultTimeSlowDuration;
    [SerializeField] private float timeSlowTransitionDuration;
    [SerializeField] private float timeHiccupTransitionDuration;
    [SerializeField] private float slowTimeScale;
    [SerializeField] private float normalTimeScale;
    
    [SerializeField] private bool unlimitedTimeSlow = false;
    [SerializeField] private bool slowTimeOnStart = false;
    [SerializeField] private bool movingStopsTimeSlow = false;
    [SerializeField] private bool useTimeBar = true;
    
    public bool TimeSlowIsUnlimited { get { return unlimitedTimeSlow; } }
    
    private Tween timeSlowTween;
    private bool isTimeSlowed = false;
    private float timeSlowTimer = 0.0f;

    private void Start()
    {
        if (slowTimeOnStart)
        {
            SlowTime();
        }
    }

    public void SlowTime()
    {
        SlowTime(defaultTimeSlowDuration);
    }

    public void SlowTime(float timeSlowDuration)
    {
        if(timeSlowDuration > 0.0f)
        {
            timeSlowTimer = timeSlowDuration;
            timeSlowTween = DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, slowTimeScale, timeSlowTransitionDuration).SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutCirc);
            HUDManager.Instance().UseVignette(true);
            isTimeSlowed = true;
        }
    }

    public void NormalizeTime()
    {
        if(isTimeSlowed == true)
        {
            timeSlowTween = DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, normalTimeScale, timeSlowTransitionDuration).SetUpdate(UpdateType.Normal, true).SetEase(Ease.InCirc);
            HUDManager.Instance().UseVignette(false);
            isTimeSlowed = false;
        }
    }

    public void TimeHiccup()
    {
        if(isTimeSlowed == true)
        {
            Time.timeScale = normalTimeScale;

            if(timeSlowTween != null)
            {
                timeSlowTween.Kill();
            }

            timeSlowTween = DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, slowTimeScale, timeHiccupTransitionDuration).SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutQuad);
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
            if (unlimitedTimeSlow == false)
            {
                timeSlowTimer -= Time.unscaledDeltaTime;
                
                if (useTimeBar)
                {
                    HUDManager.Instance().UpdateSlowMoSliderInfo(timeSlowTimer, defaultTimeSlowDuration);
                }
            }
            
            if (movingStopsTimeSlow)
            {
                if (PlayerManager.Instance().GetPlayerMove().IsMoving)
                {
                    Time.timeScale = normalTimeScale;
                }
                else if (!PlayerManager.Instance().GetPlayerMove().IsMoving)
                {
                    Time.timeScale = slowTimeScale;
                }
            }
            
            if (timeSlowTimer <= 0.0f)
            {
                NormalizeTime();
            }
        }
    }
}
