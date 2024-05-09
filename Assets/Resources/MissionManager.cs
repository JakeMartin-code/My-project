using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private Dictionary<string, Mission> missionMap;


    private int currentPlayerLevel;
    public string activeMissionID;


    private void Awake()
    {
        Debug.Log("QuestManager Awake");
        missionMap = CreateMissionMap();
    
    }

    private void OnEnable()
    {
        Debug.Log("QuestManager enabled");
        EventsManager.instance.missionEvent.onStartMission += StartMission;
        EventsManager.instance.missionEvent.onProgressMission += ProgressMission;
        EventsManager.instance.missionEvent.onFailMission += FailedMission;
        EventsManager.instance.missionEvent.onEndMission += FinishMission;
        EventsManager.instance.onLevelUp += CheckPlayerLevel;

      

    }

    private void OnDisable()
    {
        EventsManager.instance.missionEvent.onStartMission -= StartMission;
        EventsManager.instance.missionEvent.onProgressMission -= ProgressMission;
        EventsManager.instance.missionEvent.onFailMission -= FailedMission;
        EventsManager.instance.missionEvent.onEndMission -= FinishMission;
        EventsManager.instance.onLevelUp -= CheckPlayerLevel;

        
    }

    private void Start()
    {
        foreach(Mission mission in missionMap.Values)
        {
            EventsManager.instance.missionEvent.MissionStateChanged(mission);
        }

        currentPlayerLevel = FindObjectOfType<PlayerStats>().playerLevel;
        activeMissionID = null;
    }

    private void Update()
    {
        foreach(Mission mission in missionMap.Values)
        {
            if(mission.missionState == MissionState.requirements_not_met && CheckMissionRequirements(mission))
            {
                ChangeMissionState(mission.missionInfo.id, MissionState.can_start);
            }

            if (mission.missionState == MissionState.in_progress)
            {
                MissionStep currentStep = mission.GetCurrentStep();
                if (currentStep != null)
                {
                    
                    currentStep.CheckFailureCondition();
                }
            }
        }
    }

    private void ChangeMissionState(string id, MissionState state)
    {
        Mission mission = GetMissionByID(id);
        mission.missionState = state;
        EventsManager.instance.missionEvent.MissionStateChanged(mission);
    }

    private void CheckPlayerLevel(int level)
    {
        currentPlayerLevel = level;
    }

    private bool CheckMissionRequirements(Mission mission)
    {
        bool meetsRequirements = true;

        if(currentPlayerLevel < mission.missionInfo.levelRequirement)
        {
            meetsRequirements = false;
        }

        foreach(MissionInformation prereqInfo in mission.missionInfo.prerequisits)
        {
            if(GetMissionByID(prereqInfo.id).missionState != MissionState.finished)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void StartMission(string id)
    {
        activeMissionID = id;
        Mission mission = GetMissionByID(id);
        mission.InstantiateCurrentMissionStep(this.transform);
        ChangeMissionState(mission.missionInfo.id, MissionState.in_progress);
        Debug.Log("start mission" + id);
    }

    private void ProgressMission(string id)
    {
        Mission mission = GetMissionByID(id);
        if (mission.missionState != MissionState.failed)
        {
            mission.MissionProgress();

     
            if (mission.CurrentMissionStepExists())
            {
                mission.InstantiateCurrentMissionStep(this.transform);
            }
         
            else
            {
                ChangeMissionState(mission.missionInfo.id, MissionState.can_finish);
            }
        }
        else
        {
            Debug.Log($"Attempted to progress failed mission: {id}");
        }
    }

    private void FailedMission(string id)
    {
        Mission mission = GetMissionByID(id);
        Debug.Log($"Mission failed: {id}");
        Debug.Log($"Triggering fail event for mission: {id}");
        ChangeMissionState(mission.missionInfo.id, MissionState.failed); 


        MissionTracker.Instance.RecordMissionFail(mission.missionInfo.missionType);

 
        Debug.Log(string.Format("Mission failed: {0}. Type: {1}. Total fails for this type: {2}",
            id, mission.missionInfo.missionType, MissionTracker.Instance.GetMissionTypeFails(mission.missionInfo.missionType)));

        activeMissionID = null;
    }

    private void FinishMission(string id)
    {

        Debug.Log("finish quest" + id);

        Mission mission = GetMissionByID(id);
        if (mission.missionState == MissionState.can_finish)
        {
            ClaimRewards(mission);
            ChangeMissionState(mission.missionInfo.id, MissionState.finished);

         
            MissionTracker.Instance.RecordMissionCompletion(mission.missionInfo.missionType);

         
            Debug.Log(string.Format("Mission completed: {0}. Type: {1}. Total completions for this type: {2}",
                id, mission.missionInfo.missionType, MissionTracker.Instance.GetMissionTypeCompletions(mission.missionInfo.missionType)));
            activeMissionID = null;
        }
        else
        {
            Debug.Log($"Attempted to finish mission in invalid state: {id}");
        }
    }

    private void ClaimRewards(Mission mission)
    {
        EventsManager.instance.ExperienceGained(mission.missionInfo.xpReward);
    }

    private Dictionary<string, Mission> CreateMissionMap()
    {
        MissionInformation[] allMissions = Resources.LoadAll<MissionInformation>("Missions");
        Dictionary<string, Mission> idToMissionMap = new Dictionary<string, Mission>();
        foreach(MissionInformation missionInformation in allMissions)
        {
            if(idToMissionMap.ContainsKey(missionInformation.id))
            {
                Debug.Log("Dup id found " + missionInformation.id); 
            }
            idToMissionMap.Add(missionInformation.id, new Mission(missionInformation));
        }
        return idToMissionMap;
    }

    public Mission GetMissionByID(string id)
    {
        Mission mission = missionMap[id];
        if(mission == null)
        {
            Debug.Log("id nto found in map" + id);
        }
        return mission;
    }
}
