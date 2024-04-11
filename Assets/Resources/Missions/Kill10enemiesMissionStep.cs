using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill10enemiesMissionStep : MissionStep
{
    private int enemiesKilled = 0;
    private int enemiesToKill = 1;

    private void OnEnable()
    {
        EnemyManager.EnemyKilled += EnemyKilled;
        EventsManager.instance.onPlayerDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        EnemyManager.EnemyKilled -= EnemyKilled;
        EventsManager.instance.onPlayerDeath -= PlayerDeath;
    }

    public override float ProgressPercentage
    {
        get { return (float)enemiesKilled / enemiesToKill; }
    }

    public override void CheckFailureCondition()
    {
        // This method will be called by the QuestManager or Mission logic to check for failure conditions.
    }

    private void PlayerDeath()
    {
        EventsManager.instance.missionEvent.FailMission(missionID);
  
      
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
