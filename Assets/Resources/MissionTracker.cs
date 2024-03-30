using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissionTracker : MonoBehaviour
{
    
    public static MissionTracker Instance { get; private set; }
    private Dictionary<MissionType, int> missionTypeCompletions = new Dictionary<MissionType, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RecordMissionCompletion(MissionType missionType)
    {
        if (!missionTypeCompletions.ContainsKey(missionType))
        {
            missionTypeCompletions[missionType] = 0;
        }
        missionTypeCompletions[missionType] += 1;
    }

    public int GetMissionTypeCompletions(MissionType missionType)
    {
        if (missionTypeCompletions.TryGetValue(missionType, out int completions))
        {
            return completions;
        }
        return 0;
    }

}