using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

public class MissionEvents
{
    public event Action<String> onStartMission;
    public event Action<String> onProgressMission;
    public event Action<String> onEndMission;
    public event Action<Mission> onMissionStateChanged;

    public void StartMission(string id)
    {
        if(onStartMission != null)
        {
            onStartMission(id);
        }
    }

    public void ProgressMission(string id)
    {
        if (onProgressMission != null)
        {
            onProgressMission(id);
        }
    }

    public void EndMission(string id)
    {
        if (onEndMission != null)
        {
            onEndMission(id);
        }
    }

    public void MissionStateChanged(Mission mission)
    {
        if (onStartMission != null)
        {
            onMissionStateChanged(mission);
        }
    }
}
