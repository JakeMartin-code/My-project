using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("start mission" + id);
    }

    private void ProgressMission(string id)
    {
        Debug.Log("progroess mission" + id);
    }

    private void FinishMission(string id)
    {
        Debug.Log("end mission" + id);
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

    private Mission GetMissionByID(string id)
    {
        Mission mission = missionMap[id];
        if(mission == null)
        {
            Debug.Log("id nto found in map" + id);
        }
        return mission;
    }
}
