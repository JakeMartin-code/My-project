using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Mission> missionMap;


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
    }

    private void OnDisable()
    {
        EventsManager.instance.missionEvent.onStartMission -= StartMission;
        EventsManager.instance.missionEvent.onProgressMission -= ProgressMission;
        EventsManager.instance.missionEvent.onEndMission -= FinishMission;
    }

    private void Start()
    {
        foreach(Mission mission in missionMap.Values)
        {
            EventsManager.instance.missionEvent.MissionStateChanged(mission);
        }
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
