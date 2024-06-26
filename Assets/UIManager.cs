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
    private MissionManager missionManager;


    public TMP_Dropdown primaryDropdown;
    public TMP_Dropdown secondaryDropdown;
    public TMP_Dropdown heavyDropdown;
    public WeaponManager weaponManager;

    private void Awake()
    {
        missionManager = FindObjectOfType<MissionManager>();
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

    
    }

    public void OnSecondaryWeaponChanged(int index)
    {
        int pickedEntryIndex = secondaryDropdown.value;
        WeaponBehavior newWeapon = weaponManager.secondaryInventory[pickedEntryIndex];
        weaponManager.EquipWeaponToSlot(newWeapon, WeaponSlot.Secondary);

    }

    public void OnHeavyWeaponChanged(int index)
    {
        int pickedEntryIndex = heavyDropdown.value;
        WeaponBehavior newWeapon = weaponManager.heavyInventory[pickedEntryIndex];
        weaponManager.EquipWeaponToSlot(newWeapon, WeaponSlot.Heavy);
        
    }

    private void UpdateMissionUI(string id)
    {
        if (missionManager != null)
        {
            Mission mission = missionManager.GetMissionByID(id);
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
     
        missionNameText.SetText("");
        missionDescriptionText.SetText("");
        missionPercentCompleted.SetText("");
    }

    private void ClearMissionUIFailed(string id)
    {
      
        missionNameText.SetText("");
        missionDescriptionText.SetText("");
        missionPercentCompleted.SetText("");
    }

    public void Update()
    {
        perkTreeSkillPoints.SetText("skill points " + playerStats.perkPoints);
        perkTreeCurrentLevel.SetText("level " + playerStats.playerLevel);
        hudCurrentLevel.SetText("" + playerStats.playerLevel);
    }
}
