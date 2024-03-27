using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class MissionPoint : MonoBehaviour
{
    private bool playerIsNear = false;

    [SerializeField] private MissionInformation missionInfoForPoint;
    private string missionID;
    private MissionState currentMissionState;
    

    private void Awake()
    {
        missionID = missionInfoForPoint.id;
    }

    private void OnEnable()
    {
        EventsManager.instance.missionEvent.onMissionStateChanged += MissionStateCanged;
    }

    private void OnDisable()
    {
        EventsManager.instance.missionEvent.onMissionStateChanged -= MissionStateCanged;
    }

    private void PointInteracted()
    {
        if(!playerIsNear)
        {
            return;
        }

        EventsManager.instance.missionEvent.StartMission(missionID);
        EventsManager.instance.missionEvent.ProgressMission(missionID);
        EventsManager.instance.missionEvent.EndMission(missionID);
    }

   

    private void MissionStateCanged(Mission mission)
    {
        if(mission.missionInfo.id.Equals(missionID))
        {
            currentMissionState = mission.missionState;
            Debug.Log("quest id: " + missionID + "state updated: " + currentMissionState);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerIsNear = true;
            Debug.Log("entered mission zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }

}
