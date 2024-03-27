using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Mission/Mission")]
public class MissionInformation : ScriptableObject
{
   
    
  
    public MissionType missionType;
    public int killAmount;
    public float suriveTimer;
    public GameObject interactionPoint;

    public IMissionProgress progressTracker;

    public enum MissionType
    {
        Kill,
        Interact,
        Survive
    }



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


    private void OnEnable()
    {
        // Initialize the appropriate progress tracker based on the mission type
        switch (missionType)
        {
            case MissionType.Kill:
                progressTracker = new KillProgress(killAmount);
                
                break;
            case MissionType.Interact:
                progressTracker = new InteractionProgress();
            
                break;
            case MissionType.Survive:
                progressTracker = new SurviveProgress(suriveTimer);
               
                break;
        }
    }
}
