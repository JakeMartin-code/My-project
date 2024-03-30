using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionStep : MonoBehaviour
{
    public abstract float ProgressPercentage { get; }

    private bool isFinished = false;

    public string missionID;

    public void InitaliseMissionStep(string missionID)
    {
        this.missionID = missionID;
    }

    protected void FinishQuestStep()
    {
        if(!isFinished)
        {
            isFinished = true;
            EventsManager.instance.missionEvent.ProgressMission(missionID);
            Destroy(this.gameObject);
        }
    }
}
