using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Mission
{

    public MissionInformation missionInfo;
    public MissionState missionState;
    private int currentMissionStepIndex;
    private MissionStep currentMissionStep; 

    public Mission(MissionInformation missionInformation)
    {
        this.missionInfo = missionInformation;
        this.missionState = MissionState.requirements_not_met;
        this.currentMissionStepIndex = 0;
    }

    public void MissionProgress()
    {
        currentMissionStepIndex++;
    }

    public bool CurrentMissionStepExists()
    {
        return (currentMissionStepIndex < missionInfo.missionStepPrefabs.Length);
    }

    public void InstantiateCurrentMissionStep(Transform parentTransform)
    {
        GameObject missionStepPrefab = GetCurrentMissionStepPrefab();
        if (missionStepPrefab != null)
        {
            GameObject instantiatedStep = Object.Instantiate(missionStepPrefab, parentTransform);
            currentMissionStep = instantiatedStep.GetComponent<MissionStep>(); 
            if (currentMissionStep != null)
            {
                currentMissionStep.InitaliseMissionStep(missionInfo.id);
            }
            else
            {
                Debug.LogError("Instantiated mission step prefab does not contain a MissionStep component.");
            }
        }
    }

    private GameObject GetCurrentMissionStepPrefab()
    {
        if (CurrentMissionStepExists())
        {
            return missionInfo.missionStepPrefabs[currentMissionStepIndex];
        }
        return null;
    }

 
    public MissionStep GetCurrentStep()
    {
        return currentMissionStep;
    }
}
