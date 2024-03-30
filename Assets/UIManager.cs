using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionNameText;
    [SerializeField] private TextMeshProUGUI missionDescriptionText;
    [SerializeField] private TextMeshProUGUI missionPercentCompleted;
    [SerializeField] private Slider missionProgressSlider;


   
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
        EventsManager.instance.missionEvent.onMissionProgress += UpdateMissionPercentage;
    }

    private void OnDisable()
    {
        EventsManager.instance.missionEvent.onStartMission -= UpdateMissionUI;
        EventsManager.instance.missionEvent.onEndMission -= UpdateMissionUI;
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
               
                missionNameText.text = mission.missionInfo.name;
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
        missionNameText.SetText("");
        missionDescriptionText.SetText("");
        missionPercentCompleted.SetText("");
    }
}
