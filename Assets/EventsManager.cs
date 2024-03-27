using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{

    public static EventsManager instance { get; private set; }

    public MissionEvents missionEvent;

    private void Awake()
    {
        if(instance!= this)
        {
            Debug.Log("multiple events managers exist");
        }
        instance = this;

        missionEvent = new MissionEvents();

    }

 

}
