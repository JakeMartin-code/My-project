using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;

public class MissionManager : MonoBehaviour
{
    public List<MissionInformation> allMissions = new List<MissionInformation>();
    [SerializeField] private List<MissionInformation> activeMissions = new List<MissionInformation>();
    public int maxActiveMissions = 3;

    public UnityEvent<MissionInformation> onMissionStarted = new UnityEvent<MissionInformation>();
    public UnityEvent<MissionInformation> onMissionProgressing = new UnityEvent<MissionInformation>();
    public UnityEvent<MissionInformation> onMissionFinished = new UnityEvent<MissionInformation>();

    public TextMeshProUGUI mission1;
    public TextMeshProUGUI mission2;
    public TextMeshProUGUI mission3;



    enum MissionState { NotStarted, Progressing, Finished }
    MissionState currentState = MissionState.NotStarted;

    private List<MissionInformation> availableMissions = new List<MissionInformation>();

    private void Start()
    {
        ClearMissionTexts();
        StartRandomMissions();
    }

    void OnEnable()
    {
        onMissionFinished.AddListener(HandleMissionCompletedExpanded);
        onMissionStarted.AddListener(StartMissionListener);
        Enemy.EnemyKilled += OnEnemyKilled;
    }

    void OnDisable()
    {
        onMissionFinished.RemoveListener(HandleMissionCompletedExpanded);
        onMissionStarted.RemoveListener(StartMissionListener);
        Enemy.EnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        foreach (var mission in new List<MissionInformation>(activeMissions))
        {
            if (mission.missionType == MissionInformation.MissionType.Kill)
            {
                // Update progress without passing a hardcoded value
                if (mission.progressTracker.UpdateProgress())
                {
                    CompleteMission(mission);
                 
                }
            }
        }
    }

    private void StartRandomMissions()
    {
        PopulateAvailableMissions();
        ActivateRandomMissionsUntilMaximum();
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
            if (!activeMissions.Contains(mission))
            {
                availableMissions.Add(mission);
            }
        }
    }

    private void ActivateRandomMissionsUntilMaximum()
    {
        while (activeMissions.Count < maxActiveMissions && availableMissions.Count > 0)
        {
            MissionInformation selectedMission = SelectRandomMission();
            if (StartMission(selectedMission))
            {
                availableMissions.Remove(selectedMission); // Correctly remove after successful start.
            }
        }
    }

    public void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach (var mission in activeMissions)
        {
            if (mission.missionType == MissionInformation.MissionType.Survive)
            {
                if (mission.progressTracker is SurviveProgress surviveProgress)
                {
                    // Call UpdateProgress on SurviveProgress with deltaTime
                    surviveProgress.UpdateProgress(deltaTime);
                }
            }
        }

        UpdateMissionTexts(); // Refresh UI to show updated mission statuses
    }

    private MissionInformation SelectRandomMission()
    {
        int randomIndex = Random.Range(0, availableMissions.Count);
        return availableMissions[randomIndex];
    }

    private bool StartMission(MissionInformation mission)
    {
        if (activeMissions.Count < maxActiveMissions)
        {
            activeMissions.Add(mission);
            Debug.Log("Mission started: " + mission.missionName);
            onMissionStarted.Invoke(mission); // Adjusted to pass mission info.
            return true;
        }
        return false;
    }

    public void CompleteMission(MissionInformation completedMission)
    {
        if (activeMissions.Contains(completedMission))
        {
            activeMissions.Remove(completedMission);
            Debug.Log($"Mission completed: {completedMission.missionName}");
            onMissionFinished.Invoke(completedMission); // Adjusted to pass mission info.
            RepopulateMissions();
        }
    }

    private void ClearMissionTexts()
    {
        mission1.text = "";
        mission2.text = "";
        mission3.text = "";
    }

    private void UpdateMissionTexts()
    {
        // Clear all TextMeshPro components initially
        ClearMissionTexts();

        // Loop through each active mission
        for (int i = 0; i < activeMissions.Count && i < 3; i++)
        {
            MissionInformation mission = activeMissions[i];
            TextMeshProUGUI textComponent = GetMissionTextComponent(i);

            if (textComponent != null)
            {
                // Construct the mission text using mission name and description
                string missionText = mission.missionName + "\n" + mission.description;

                // Append specific goal information based on mission type
                switch (mission.missionType)
                {
                    case MissionInformation.MissionType.Kill:
                        if (mission.progressTracker is KillProgress killProgress)
                        {
                            missionText +=  killProgress.RemainingKills();
                        }
                        break;
                    case MissionInformation.MissionType.Interact:
                        // Interact mission logic
                        break;
                    case MissionInformation.MissionType.Survive:
                        if (mission.progressTracker is SurviveProgress surviveProgress)
                        {
                          
                            missionText += surviveProgress.RemainingTimeFormatted();
                        }
                        break;
                }

                // Append completion status if the mission is finished
                if (mission.progressTracker.IsComplete())
                {
                    missionText += " (Completed)";
                }

                // Set the text for the TextMeshPro component
                textComponent.text = missionText;
            }
        }
    }


    private TextMeshProUGUI GetMissionTextComponent(int index)
    {
        switch (index)
        {
            case 0: return mission1;
            case 1: return mission2;
            case 2: return mission3;
            default: return null;
        }
    }


    private void RepopulateMissions()
    {
        if (activeMissions.Count == 0)
        {
            StartRandomMissions();
        }
    }

    private void HandleMissionCompletedExpanded(MissionInformation mission)
    {
        // Additional logic upon mission completion can be added here.
    }

    // Added expanded method for mission started listener to replace lambda expression
    private void StartMissionListener(MissionInformation mission)
    {
        UpdateMissionTexts();
    }
}
