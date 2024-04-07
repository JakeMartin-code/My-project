using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionNameText;
    [SerializeField] private TextMeshProUGUI missionDescriptionText;
    [SerializeField] private TextMeshProUGUI missionPercentCompleted;
    [SerializeField] private TextMeshProUGUI perkTreeSkillPoints;
    [SerializeField] private TextMeshProUGUI perkTreeCurrentLevel;
    [SerializeField] private Slider missionProgressSlider;

    public PlayerStats playerStats;

   
    private QuestManager questManager;

    private void Awake()
    {
        questManager = FindObjectOfType<QuestManager>();
    }

    private void OnEnable()
    {
        EventsManager.instance.missionEvent.onStartMission += UpdateMissionUI;
        EventsManager.instance.missionEvent.onProgressMission += UpdateMissionUI;
        EventsManager.instance.missionEvent.onEndMission += ClearMissionUI;
        EventsManager.instance.missionEvent.onFailMission += ClearMissionUIFailed ;
        EventsManager.instance.missionEvent.onMissionProgress += UpdateMissionPercentage;


    }

    private void OnDisable()
    {
        EventsManager.instance.missionEvent.onStartMission -= UpdateMissionUI;
        EventsManager.instance.missionEvent.onEndMission -= UpdateMissionUI;
        EventsManager.instance.missionEvent.onFailMission -= ClearMissionUIFailed;
        EventsManager.instance.missionEvent.onProgressMission -= UpdateMissionUI;
        EventsManager.instance.missionEvent.onMissionProgress -= UpdateMissionPercentage;

    }

    private void UpdateMissionUI(string id)
    {
        if(questManager != null)
        {
            Mission mission = questManager.GetMissionByID(id);
            if (mission != null)
            {
               
                missionNameText.text = mission.missionInfo.missionName;
                missionDescriptionText.text = mission.missionInfo.description;
            }
        }
    }

    private void UpdateMissionPercentage(string missionID, float progress)
    {
        missionPercentCompleted.text = string.Format("{0:0}%", progress * 100);
    }

    private void ClearMissionUI(string id)
    {
        Debug.Log($"Clearing UI for mission: {id}");
        missionNameText.SetText("");
        missionDescriptionText.SetText("");
        missionPercentCompleted.SetText("");
    }

    private void ClearMissionUIFailed(string id)
    {
        Debug.Log($"Clearing failed UI for mission : {id}");
        missionNameText.SetText("");
        missionDescriptionText.SetText("");
        missionPercentCompleted.SetText("");
    }

    public void Update()
    {
        perkTreeSkillPoints.SetText("" + playerStats.perkPoints);
        perkTreeCurrentLevel.SetText("" + playerStats.playerLevel);
    }
}
