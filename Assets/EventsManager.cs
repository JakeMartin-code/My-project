using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{

    public static EventsManager instance { get; private set; }

    public MissionEvents missionEvent;
    public event Action<int> onLevelUp;

 
    private void Awake()
    {
        Debug.Log("EventsManager Awake");

        if (instance != null && instance != this)
        {
            Debug.LogWarning("Multiple EventsManager instances detected. Destroying duplicate.");
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

}
