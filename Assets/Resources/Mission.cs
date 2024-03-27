using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Mission
{
    public MissionInformation missionInfo;
    public MissionState missionState;
    private int currentMissionStepIndex;

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
        if(missionStepPrefab != null)
        {
            MissionStep missionStep = Object.Instantiate<GameObject>(missionStepPrefab, parentTransform).GetComponent<MissionStep>();
            missionStep.InitaliseMissionStep(missionInfo.id);
        }
    }

    private GameObject GetCurrentMissionStepPrefab()
    {
        GameObject missionStepPrefab = null;
        if (CurrentMissionStepExists())
        {
            missionStepPrefab = missionInfo.missionStepPrefabs[currentMissionStepIndex];
        }
        else
        {
            Debug.LogWarning("quest step now out of range" + missionInfo.id + "step index" + currentMissionStepIndex);

        }

        return missionStepPrefab;
    }
}
