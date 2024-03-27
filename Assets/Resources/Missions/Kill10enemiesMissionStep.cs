using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MissionStep
{
    public int enemiesKilled = 0;
    public int enemiesToKill = 3;

    private void OnEnable()
    {
        Enemy.EnemyKilled += EnemyKilled;
    }

    private void OnDisable()
    {
        Enemy.EnemyKilled -= EnemyKilled;
    }

    private void EnemyKilled(Enemy enemy)
    {
        if(enemiesKilled < enemiesToKill)
        {
            enemiesKilled++;
        }

        if (enemiesKilled >= enemiesToKill)
        {
            FinishQuestStep();
        }

    }

}
