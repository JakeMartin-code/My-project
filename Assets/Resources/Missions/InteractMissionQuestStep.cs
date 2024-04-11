using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMissionQuestStep : MissionStep
{
    public override float ProgressPercentage => throw new System.NotImplementedException();

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            FinishQuestStep();
        }
    }

    public override void CheckFailureCondition()
    {
        // This method will be called by the QuestManager or Mission logic to check for failure conditions.
    }

}
