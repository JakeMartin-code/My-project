using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MissionPoint : MonoBehaviour
{
    private bool playerIsNear = false;

    [SerializeField] private MissionInformation missionInfoForPoint;
    private string missionID;
    private MissionState currentMissionState;

    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool endPoint = true;

    public MissionIcons missionIcons;
    public MissionManager missionManager;

    private void Awake()
    {
  
        missionID = missionInfoForPoint.id;
       
    }

    private void OnEnable()
    {
       
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

        if(currentMissionState.Equals(MissionState.can_start) && startPoint && missionManager.activeMissionID == null)
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
            missionIcons.SetState(currentMissionState, startPoint, endPoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerIsNear = true;
      
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
