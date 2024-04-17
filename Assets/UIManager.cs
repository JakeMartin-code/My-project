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
    [SerializeField] private TextMeshProUGUI hudCurrentLevel;
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
        EventsManager.instance.missionEvent.onFailMission += ClearMissionUIFailed;
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
            if (weapon != null && weapon.weaponStats != null)
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
        int pickedEntryIndex = primaryDropdown.value;
        WeaponBehavior newWeapon = weaponManager.primaryInventory[pickedEntryIndex];
        weaponManager.EquipWeaponToSlot(newWeapon, WeaponSlot.Primary);
        // Log for debugging
        Debug.Log($"New primary weapon equipped: {newWeapon.weaponStats.weaponName}");
    }

    // Continue with similar modifications for other dropdown handlers
    public void OnSecondaryWeaponChanged(int index)
    {
        int pickedEntryIndex = secondaryDropdown.value;
        WeaponBehavior newWeapon = weaponManager.secondaryInventory[pickedEntryIndex];
        weaponManager.EquipWeaponToSlot(newWeapon, WeaponSlot.Secondary);
        // Log for debugging
        Debug.Log($"New secondary weapon equipped: {newWeapon.weaponStats.weaponName}");
    }

    public void OnHeavyWeaponChanged(int index)
    {
        int pickedEntryIndex = heavyDropdown.value;
        WeaponBehavior newWeapon = weaponManager.heavyInventory[pickedEntryIndex];
        weaponManager.EquipWeaponToSlot(newWeapon, WeaponSlot.Heavy);
        // Log for debugging
        Debug.Log($"New heavy weapon equipped: {newWeapon.weaponStats.weaponName}");
    }

    private void UpdateMissionUI(string id)
    {
        if (questManager != null)
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
        hudCurrentLevel.SetText("" + playerStats.playerLevel);
    }
}
