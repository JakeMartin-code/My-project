using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;


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


    public TMP_Dropdown primaryDropdown;
    public TMP_Dropdown secondaryDropdown;
    public TMP_Dropdown heavyDropdown;
    public WeaponManager weaponManager;

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

    private void Start()
    {
        PopulateDropdowns();
    }

    public void PopulateDropdowns()
    {
        primaryDropdown.ClearOptions();
        List<string> primaryOptions = new List<string>();
        foreach (WeaponBehavior weapon in weaponManager.primaryInventory)
        {
            if (weapon == null)
            {
                Debug.LogWarning("A WeaponBehavior in primary inventory is null.");
            }
            else if (weapon.weaponStats == null)
            {
                Debug.LogWarning("WeaponStats is null for a WeaponBehavior in primary inventory. Weapon object: " + weapon.gameObject.name);
            }
            else
            {
                primaryOptions.Add(weapon.weaponStats.weaponName);
            }
        }
        primaryDropdown.AddOptions(primaryOptions);

        secondaryDropdown.ClearOptions();
        List<string> secondaryOptions = new List<string>();
        foreach (WeaponBehavior weapon in weaponManager.secondaryInventory)
        {
            secondaryOptions.Add(weapon.weaponStats.weaponName);
        }
        secondaryDropdown.AddOptions(secondaryOptions);

        heavyDropdown.ClearOptions();
        List<string> heavyOptions = new List<string>();
        foreach (WeaponBehavior weapon in weaponManager.heavyInventory)
        {
            heavyOptions.Add(weapon.weaponStats.weaponName);
        }
        heavyDropdown.AddOptions(heavyOptions);
    }

    public void UpdateWeaponDropdowns()
    {
        PopulateDropdowns();
    }

    public void OnPrimaryWeaponChanged(int index)
    {
        weaponManager.EquipWeaponToSlot(weaponManager.primaryInventory[index], WeaponSlot.Primary);
    }

    public void OnSecondaryWeaponChanged(int index)
    {
        weaponManager.EquipWeaponToSlot(weaponManager.secondaryInventory[index], WeaponSlot.Secondary);
    }

    public void OnHeavyWeaponChanged(int index)
    {
        weaponManager.EquipWeaponToSlot(weaponManager.heavyInventory[index], WeaponSlot.Heavy);
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
