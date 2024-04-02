using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freexp : MonoBehaviour
{

    public int xpReward;

    public void RewardXP()
    {
        PlayerStats player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (player != null)
        {
            player.GainXP(xpReward);
           
        }
    }
}
