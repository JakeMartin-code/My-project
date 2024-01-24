using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freexp : MonoBehaviour
{

    public int xpReward;

    public void RewardXP()
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            player.GainXP(xpReward);
            Debug.Log("Player gained " + xpReward + " XP from defeating this enemy!");
        }
    }
}
