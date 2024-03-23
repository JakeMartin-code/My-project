using UnityEngine;
using System.Collections.Generic;

public class MissionProgres : MonoBehaviour
{
    private Dictionary<Mission, int> missionProgress = new Dictionary<Mission, int>();

    public void InitializeMission(Mission mission)
    {
        if (!missionProgress.ContainsKey(mission))
        {
            missionProgress.Add(mission, mission.goalAmount);
            // Optionally, trigger any mission started logic or UI updates here
        }
    }

    public void UpdateProgress(Mission mission)
    {
        if (missionProgress.ContainsKey(mission))
        {
            missionProgress[mission]--;

            if (missionProgress[mission] <= 0)
            {
                CompleteMission(mission);
            }
            else
            {
                // Optionally, update progress UI here
                //MissionManager.Instance.OnMissionProgressing.Invoke();
            }
        }
    }

    void CompleteMission(Mission mission)
    {
        missionProgress.Remove(mission);
        // Trigger completion logic, rewards, UI updates, etc.
       // MissionManager.Instance.OnMissionFinished.Invoke();
    }
}
