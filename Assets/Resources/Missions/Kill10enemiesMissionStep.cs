using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MissionStep
{
    private int enemiesKilled = 0;
    private int enemiesToKill = 3;

    private void OnEnable()
    {
        EnemyManager.EnemyKilled += EnemyKilled;
    }

    private void OnDisable()
    {
        EnemyManager.EnemyKilled -= EnemyKilled;
    }

    public override float ProgressPercentage
    {
        get { return (float)enemiesKilled / enemiesToKill; }
    }

    private void EnemyKilled(EnemyManager enemy)
    {
       
        if (enemy is MeleeEnemy) 
        {
            if (enemiesKilled < enemiesToKill)
            {
                enemiesKilled++;
                EventsManager.instance.missionEvent.UpdateMissionProgress(missionID, this.ProgressPercentage);
            }

            if (enemiesKilled >= enemiesToKill)
            {
                FinishQuestStep();
            }
        }
    }
}
