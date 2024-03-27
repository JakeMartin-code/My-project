using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionIcons : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject requirementsNotMetToStartIcon;
    [SerializeField] private GameObject canStartIcon;
    [SerializeField] private GameObject requirementsNotMetToFinishIcon;
    [SerializeField] private GameObject canFinishIcon;

    public void SetState(MissionState newState, bool startPoint, bool finishPoint)
    {
        // set all to inactive
        requirementsNotMetToStartIcon.SetActive(false);
        canStartIcon.SetActive(false);
        requirementsNotMetToFinishIcon.SetActive(false);
        canFinishIcon.SetActive(false);

        // set the appropriate one to active based on the new state
        switch (newState)
        {
            case MissionState.requirements_not_met:
                if (startPoint) { requirementsNotMetToStartIcon.SetActive(true); }
                break;
            case MissionState.can_start:
                if (startPoint) { canStartIcon.SetActive(true); }
                break;
            case MissionState.in_progress:
                if (finishPoint) { requirementsNotMetToFinishIcon.SetActive(true); }
                break;
            case MissionState.can_finish:
                if (finishPoint) { canFinishIcon.SetActive(true); }
                break;
            case MissionState.finished:
                break;
            default:
                Debug.LogWarning("Quest State not recognized by switch statement for quest icon: " + newState);
                break;
        }
    }
}