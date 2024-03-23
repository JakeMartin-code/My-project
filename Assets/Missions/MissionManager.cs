using UnityEngine;
using UnityEngine.Events;
using TMPro; 
using System.Collections.Generic;

public class MissionManager : MonoBehaviour
{
    public List<Mission> allMissions = new List<Mission>();
    [SerializeField] private List<Mission> activeMissions = new List<Mission>();
    public int maxActiveMissions = 3;

    public UnityEvent onMissionStarted;
    public UnityEvent onMissionProgressing;
    public UnityEvent onMissionFinished;

    public TextMeshProUGUI mission1;
    public TextMeshProUGUI mission2;
    public TextMeshProUGUI mission3;

    enum MissionState { NotStarted, Progressing, Finished }
    MissionState currentState = MissionState.NotStarted;

    private List<Mission> availableMissions = new List<Mission>(); 

    private void Start()
    {
        ClearMissionTexts();
        StartRandomMissions(); 
    }



    void OnEnable()
    {
        onMissionFinished.AddListener(HandleMissionCompleted);
        onMissionStarted.AddListener(UpdateMissionTexts);
        Enemy.EnemyKilled += OnEnemyKilled;
    }

    void OnDisable()
    {
        onMissionFinished.RemoveListener(HandleMissionCompleted);
        onMissionStarted.RemoveListener(UpdateMissionTexts);
        Enemy.EnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        // Loop through active missions to check if the killed enemy fulfills any mission objectives
        foreach (var mission in new List<Mission>(activeMissions)) 
        {
            
            if (mission.missionType == Mission.MissionType.Kill)
            {
                mission.goalAmount--; 

            
                if (mission.goalAmount <= 0)
                {
                    CompleteMission(mission);
                }
                else
                {
                   // update UI 
                  
                }
            }
        }
    }

    private void StartRandomMissions()
    {
        if (HasReachedMaximumActiveMissions())
        {
            Debug.Log("Maximum active missions reached.");
            return;
        }

        PopulateAvailableMissions();

        ActivateRandomMissionsUntilMaximum();

        if (activeMissions.Count > 0)
        {
            currentState = MissionState.Progressing;
        }
    }

    private bool HasReachedMaximumActiveMissions()
    {
        return activeMissions.Count >= maxActiveMissions;
    }

    private void PopulateAvailableMissions()
    {
        availableMissions.Clear();
        foreach (var mission in allMissions)
        {
            if (!IsMissionActive(mission))
            {
                availableMissions.Add(mission);
            }
        }
    }

    private bool IsMissionActive(Mission mission)
    {
        return activeMissions.Contains(mission);
    }

    private void ActivateRandomMissionsUntilMaximum()
    {
        while (activeMissions.Count < maxActiveMissions && availableMissions.Count > 0)
        {
            Mission selectedMission = SelectRandomMission();
            StartMission(selectedMission);
        }
    }

    private Mission SelectRandomMission()
    {
        int randomIndex = Random.Range(0, availableMissions.Count);
        Mission selectedMission = availableMissions[randomIndex];
        availableMissions.RemoveAt(randomIndex); // Remove the selected mission from available missions
        return selectedMission;
    }

    private void StartMission(Mission mission)
    {
        activeMissions.Add(mission); // Add to active missions
        Debug.Log("Mission started: " + mission.missionName);
        onMissionStarted.Invoke(); // Notifying about the mission start
    }

    public void CompleteMission(Mission completedMission)
    {
        if (activeMissions.Contains(completedMission))
        {
            activeMissions.Remove(completedMission); 
            Debug.Log($"Mission completed: {completedMission.missionName}");

         

            UpdateMissionTexts(); 
            onMissionFinished.Invoke(); 


            RepopulateMissions();
        }
    }

    private void ClearMissionTexts()
    {
        if (mission1 != null)
        {
            mission1.text = "";
        }
        if (mission2 != null)
        {
            mission2.text = "";
        }
        if (mission3 != null)
        {
            mission3.text = "";
        }
    }

    private void UpdateMissionTexts()
    {
        ClearMissionTexts();

        for (int i = 0; i < activeMissions.Count; i++)
        {
            string missionText = activeMissions[i].missionName + "\n" + activeMissions[i].description;

            if (i == 0 && mission1 != null)
            {
                mission1.text = missionText;
            }
            else if (i == 1 && mission2 != null)
            {
                mission2.text = missionText;
            }
            else if (i == 2 && mission3 != null)
            {
                mission3.text = missionText;
            }
        }
    }

    public void RepopulateMissions()
    {
        if (activeMissions.Count == 0)
        {

            StartRandomMissions();
        }
    }

    void HandleMissionCompleted()
    {
        //handle xp rewards etc 
    }
}