using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour { // La méthode Update() de Monobehaviour est appelée à chaque frame.

    public static TimeManager instance;
    public TimeOfDay timeOfDay;
    public delegate void Action();
    public event Action EveryMinuteUpdate; // // Will call event every minute (in game time)
    public UnityEvent OnQuarterUpdate = new UnityEvent();
    public Text clockUi;
    public Text timeSpeedUi;
    public bool displayTimeInConsole = false;
    [HideInInspector]
    public bool pause;
    private float fixedDeltaTime;

    private void Awake()
    {
        instance = this;
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
        timeOfDay = new TimeOfDay(7, 59);
        InvokeRepeating("UpdateTime", 5, 20); // UpdateTime will be called every 5 seconds (in game time)
    }
	

    public void UpdateTime()
    {
        timeOfDay.AddOneMinute();
        DisplayTime();
        if (EveryMinuteUpdate != null) 
            EveryMinuteUpdate();

        if (timeOfDay.IsQuarterHour()) // Will be true every 15 in game minutes
            OnQuarterUpdate.Invoke();
    }

    public void DisplayTime()
    {
        if (displayTimeInConsole)
            timeOfDay.Display();
        if (clockUi != null)
            clockUi.text = timeOfDay.ToString();
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = Mathf.Min(100f, Mathf.Max(0.25f, timeScale));
        if (timeSpeedUi != null)
            timeSpeedUi.text = string.Format("x{0}", Time.timeScale);
    }

    private void Update()
    {
        if (Input.GetButtonDown("TimeSpeed"))
        {
           SetTimeScale(Time.timeScale * 2);
        }
        if (Input.GetButtonDown("TimeSlow"))
        {
            float scale = Time.timeScale == 100 ? 64 : Time.timeScale / 2;
            SetTimeScale(scale);
        }

        if (Input.GetButtonDown("Pause"))
        {
            pause = !pause;

            if (pause)
            {
                Time.timeScale = 0.005f;
                if (timeSpeedUi != null)
                    timeSpeedUi.text = string.Format("  II");
            }
            else
            {
                Time.timeScale = 1;
                if (timeSpeedUi != null)
                    timeSpeedUi.text = string.Format("x{0}", Time.timeScale);
            }
        }
 
        // Adjust fixed delta time according to timescale
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

        /*
        if (Input.GetKeyDown("o"))
        {
            for (int i=0; i<3; i++)
            {
                timeOfDay += 5;
                On5MinutesUpdate();
                DisplayTime();
            }
            OnQuarterUpdate.Invoke();            
        }
        */
    }
}

[System.Serializable]
public class TimeOfDay
{
    public int hours;
    public int minutes; // Those  are public so they can be set in Editor
    public static TimeOfDay FirstHour = new TimeOfDay(8, 0);
    public static TimeOfDay LastHour = new TimeOfDay(18, 0);

    public TimeOfDay(int hours, int minutes)
    {
        this.minutes = minutes;
        this.hours = hours;
    }
    
    public void AddOneMinute()
    {
        minutes += 1;
        if(minutes >= 60)
        {
            minutes = 0;
            hours += 1;
            if (hours >= 24)
                hours = 0;
        }
    }

    public void Display()
    {
        
        Debug.Log(ToString());
    }

    public override string ToString()
    {
        return string.Format("{0:00}:{1:00}", hours, minutes);
    }

    public bool IsQuarterHour()
    {
        return minutes % 15 == 0;
    }

    public bool Is5Min()
    {
        return minutes % 5 == 0;
    }

    public static bool operator >=(TimeOfDay t1, TimeOfDay t2)
    {
        if (t1.hours < t2.hours)
        {
            return false;
        }
        else if (t1.hours == t2.hours && t1.minutes < t2.minutes)
        {
            return false;
        }
        else
            return true;
    }

    public static bool operator <=(TimeOfDay t1, TimeOfDay t2)
    {
        return t2 >= t1;
    }

    public static TimeOfDay operator +(TimeOfDay t1, int minutes)
    {
        int resultMinutes = (t1.minutes + minutes % 60) % 60;
        int resultHours = (t1.hours + minutes/60 + (t1.minutes + minutes % 60) / 60) % 24;
        return new TimeOfDay(resultHours, resultMinutes);
    }

    public TimeOfDay Copy()
    {
        return new TimeOfDay(hours, minutes);
    }

}
