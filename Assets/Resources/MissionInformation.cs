using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Mission/Mission")]
public class MissionInformation : ScriptableObject
{
   
    


    [field: SerializeField] public string id { get; private set; }


    public string missionName;
    public string description;
    public int levelRequirement;
    public MissionInformation[] prerequisits;
    public GameObject[] missionStepPrefabs;
    public int xpReward;



    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif

    }
}
