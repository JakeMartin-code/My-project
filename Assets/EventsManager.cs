using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{

    public static EventsManager instance { get; private set; }

    public MissionEvents missionEvent;
    public event Action<int> onLevelUp;
    public event Action<int> onXPGained;

    private void Awake()
    {
      

        if (instance != null && instance != this)
        {
           
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); 
        missionEvent = new MissionEvents();
    }


    public void LevelUp(int newLevel)
    {
        onLevelUp?.Invoke(newLevel);
    }

    public void ExperienceGained(int xp)
    {
        onXPGained?.Invoke(xp);
    }
}
