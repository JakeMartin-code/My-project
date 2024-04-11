using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSurvivalMission : MissionStep
{
    private float timeSurvived = 0;
    public float timeToSurvive = 5;


    public override float ProgressPercentage
    {
        get { return (float)timeSurvived / timeToSurvive; }
    }

    public void Update()
    {
        timeSurvived += Time.deltaTime;
        if(timeSurvived >= timeToSurvive)
        {
            FinishQuestStep();
            EventsManager.instance.missionEvent.UpdateMissionProgress(missionID, this.ProgressPercentage);
        }
    }

    public override void CheckFailureCondition()
    {
        // This method will be called by the QuestManager or Mission logic to check for failure conditions.
    }
}
