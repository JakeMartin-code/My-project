using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Mission> missionMap;

    // start requirements
    private int currentPlayerLevel;



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
        EventsManager.instance.missionEvent.onEndMission += FinishMission;
        EventsManager.instance.onLevelUp += CheckPlayerLevel;
    }

    private void OnDisable()
    {
        EventsManager.instance.missionEvent.onStartMission -= StartMission;
        EventsManager.instance.missionEvent.onProgressMission -= ProgressMission;
        EventsManager.instance.missionEvent.onEndMission -= FinishMission;
        EventsManager.instance.onLevelUp -= CheckPlayerLevel;
    }

    private void Start()
    {
        foreach(Mission mission in missionMap.Values)
        {
            EventsManager.instance.missionEvent.MissionStateChanged(mission);
        }

        currentPlayerLevel = FindObjectOfType<PlayerMovement>().playerLevel;
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

    private void FinishMission(string id)
    {

        Debug.Log("finish quest" + id);

        Mission mission = GetMissionByID(id);
        ClaimRewards(mission);
        ChangeMissionState(mission.missionInfo.id, MissionState.finished);
    }

    private void ClaimRewards(Mission mission)
    {
        Debug.Log($"Claiming rewards for mission: {mission.missionInfo.id}, XP: {mission.missionInfo.xpReward}");
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
