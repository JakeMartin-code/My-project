using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{

    public static EventsManager instance { get; private set; }

    public MissionEvents missionEvent;

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
        DontDestroyOnLoad(gameObject); // Optional: Only if you want it to persist across scene loads.
        missionEvent = new MissionEvents();
    }
}
