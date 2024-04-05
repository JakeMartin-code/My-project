using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Mission> missionMap;

    // start requirements
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

        EventsManager.instance.onPlayerDeath += HandlePlayerDeath;

    }

    private void OnDisable()
    {
        EventsManager.instance.missionEvent.onStartMission -= StartMission;
        EventsManager.instance.missionEvent.onProgressMission -= ProgressMission;
        EventsManager.instance.missionEvent.onFailMission -= FailedMission;
        EventsManager.instance.missionEvent.onEndMission -= FinishMission;
        EventsManager.instance.onLevelUp -= CheckPlayerLevel;

        EventsManager.instance.onPlayerDeath -= HandlePlayerDeath;
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

        // move on to the next step
        mission.MissionProgress();

        // if there are more steps, instantiate the next one
        if (mission.CurrentMissionStepExists())
        {
            mission.InstantiateCurrentMissionStep(this.transform);
        }
        // if there are no more steps, then we've finished all of them for this quest
        else
        {
            ChangeMissionState(mission.missionInfo.id, MissionState.can_finish);
        }
    }

    private void FailedMission(string id)
    {
        Mission mission = GetMissionByID(id);
        Debug.Log($"Mission failed: {id}");
        Debug.Log($"Triggering fail event for mission: {id}");
        ChangeMissionState(mission.missionInfo.id, MissionState.failed); // Set to failed

        // Record the mission completion in the MissionTracker
        MissionTracker.Instance.RecordMissionFail(mission.missionInfo.missionType);

        // Log for debugging purposes using string.Format
        Debug.Log(string.Format("Mission failed: {0}. Type: {1}. Total fails for this type: {2}",
            id, mission.missionInfo.missionType, MissionTracker.Instance.GetMissionTypeFails(mission.missionInfo.missionType)));

        activeMissionID = null;
    }

    private void FinishMission(string id)
    {

        Debug.Log("finish quest" + id);

        Mission mission = GetMissionByID(id);

        ClaimRewards(mission);
        ChangeMissionState(mission.missionInfo.id, MissionState.finished);

        // Record the mission completion in the MissionTracker
        MissionTracker.Instance.RecordMissionCompletion(mission.missionInfo.missionType);

        // Log for debugging purposes using string.Format
        Debug.Log(string.Format("Mission completed: {0}. Type: {1}. Total completions for this type: {2}",
            id, mission.missionInfo.missionType, MissionTracker.Instance.GetMissionTypeCompletions(mission.missionInfo.missionType)));
        activeMissionID = null;
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

    private void HandlePlayerDeath()
    {
        Debug.Log("handling player death");
        foreach (var mission in missionMap.Values)
        {
            if (mission.missionInfo.missionType == MissionType.Kill && mission.missionState == MissionState.in_progress)
            {
                FailedMission(mission.missionInfo.id);
                EventsManager.instance.missionEvent.FailMission(mission.missionInfo.id);

            }
        }
    }
}
