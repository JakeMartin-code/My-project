using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class MissionPoint : MonoBehaviour
{
    private bool playerIsNear = false;

    [SerializeField] private MissionInformation missionInfoForPoint;
    private string missionID;
    private MissionState currentMissionState;

    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool endPoint = true;

    //private MissionIcons missionIcons;

    private void Awake()
    {
        Debug.Log("MissionPoint Awake");
        missionID = missionInfoForPoint.id;
       // missionIcons.GetComponentInChildren<MissionIcons>();
    }

    private void OnEnable()
    {
        Debug.Log("MissionPoint enabled");
        PlayerMovement.OnInteractKeyPressed += HandleInteractKeyPress;
        EventsManager.instance.missionEvent.onMissionStateChanged += MissionStateCanged;
    }

    private void OnDisable()
    {
        PlayerMovement.OnInteractKeyPressed -= HandleInteractKeyPress;
        EventsManager.instance.missionEvent.onMissionStateChanged -= MissionStateCanged;
    }

    private void HandleInteractKeyPress()
    {
        PointInteracted();
    }

    private void PointInteracted()
    {
        if(!playerIsNear)
        {
            return;
        }

        if(currentMissionState.Equals(MissionState.can_start) && startPoint)
        {
            EventsManager.instance.missionEvent.StartMission(missionID);
        }
        else if(currentMissionState.Equals(MissionState.can_finish) && endPoint)
        {
            EventsManager.instance.missionEvent.EndMission(missionID);
        }    
    }

    private void MissionStateCanged(Mission mission)
    {
        if(mission.missionInfo.id.Equals(missionID))
        {
            currentMissionState = mission.missionState;
            //missionIcons.SetState(currentMissionState, startPoint, endPoint);
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
