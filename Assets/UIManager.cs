using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionNameText;
    [SerializeField] private TextMeshProUGUI missionDescriptionText;
    [SerializeField] private Slider missionProgressSlider;


    // Reference to the QuestManager
    private QuestManager questManager;

    private void Awake()
    {
        // Find the QuestManager instance in the scene
        questManager = FindObjectOfType<QuestManager>();
    }

    private void OnEnable()
    {
        EventsManager.instance.missionEvent.onStartMission += UpdateMissionUI;
        EventsManager.instance.missionEvent.onEndMission += ClearMissionUI;
    }

    private void OnDisable()
    {
        EventsManager.instance.missionEvent.onStartMission -= UpdateMissionUI;
        EventsManager.instance.missionEvent.onEndMission -= UpdateMissionUI;

    }

    private void UpdateMissionUI(string id)
    {
        if(questManager != null)
        {
            Mission mission = questManager.GetMissionByID(id);
            if (mission != null)
            {
                // Update UI elements with mission information
                missionNameText.text = mission.missionInfo.name;
                missionDescriptionText.text = mission.missionInfo.description;
                // For the progress, you need to determine how you'll represent it, e.g., steps completed vs total steps
            }
        }
    }

    private void ClearMissionUI(string id)
    {
        missionNameText.SetText("");
        missionDescriptionText.SetText("");
    }


}
