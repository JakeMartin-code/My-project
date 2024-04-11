using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill10RangeEnemiesMission : MissionStep
{
    private int enemiesKilled = 0;
    public int enemyGoal = 1;

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
        get { return (float)enemiesKilled / enemyGoal; }
    }

    private void EnemyKilled(EnemyManager enemy)
    {

        if (enemy is RangeEnemy)
        {
            if (enemiesKilled < enemyGoal)
            {
                enemiesKilled++;
                EventsManager.instance.missionEvent.UpdateMissionProgress(missionID, this.ProgressPercentage);
            }

            if (enemiesKilled >= enemyGoal)
            {
                FinishQuestStep();
            }
        }
    }

    public override void CheckFailureCondition()
    {
        // This method will be called by the QuestManager or Mission logic to check for failure conditions.
    }
}
