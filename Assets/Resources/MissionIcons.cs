using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionIcons : MonoBehaviour
{
    [SerializeField] private GameObject requirementNotMetIcon;
    [SerializeField] private GameObject canStartIcon;
   


    

    public void SetState(MissionState newState, bool startPoint, bool finishPoint)
    {
        // set all to inactive
        requirementNotMetIcon.SetActive(false);
        canStartIcon.SetActive(false);
      

        // set the appropriate one to active based on the new state
        switch (newState)
        {
            case MissionState.requirements_not_met:
                if (startPoint) { requirementNotMetIcon.SetActive(true); }
                break;
            case MissionState.can_start:
                if (startPoint) { canStartIcon.SetActive(true); }
                break;
            default:
                Debug.LogWarning("Quest State not recognized by switch statement for quest icon: " + newState);
                break;
        }
    }
}
