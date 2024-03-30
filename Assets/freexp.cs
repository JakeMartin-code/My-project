using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freexp : MonoBehaviour
{

    public int xpReward;

    public void RewardXP()
    {
        PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.GainXP(xpReward);
           
        }
    }
}
