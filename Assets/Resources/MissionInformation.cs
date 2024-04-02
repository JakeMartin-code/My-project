using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MissionType
{
    Kill,
    Survival,
    interact
   
}

[CreateAssetMenu(fileName = "NewMission", menuName = "Mission/Mission")]
public class MissionInformation : ScriptableObject
{
  

    [field: SerializeField] public string id { get; private set; }


    public string missionName;
    public string description;
    public int levelRequirement;
    public MissionInformation[] prerequisits;
    public GameObject[] missionStepPrefabs;
    public MissionType missionType;
    public int xpReward;
    public GameObject weaponReward;



    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif

    }
}
