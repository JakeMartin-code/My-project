using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Mission/Mission")]
public class Mission : ScriptableObject
{
    public string missionName;
    public string description;
    public int xpReward;
    public MissionType missionType;
    public int goalAmount; 

    
    public enum MissionType
    {
        Kill 
    }
}
