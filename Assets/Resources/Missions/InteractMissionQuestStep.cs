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
}
