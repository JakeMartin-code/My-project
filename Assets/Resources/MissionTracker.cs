using System.Collections.Generic;
using UnityEngine;

public class MissionTracker : MonoBehaviour
{
    
    public static MissionTracker Instance { get; private set; }
    public Dictionary<MissionType, int> missionTypeCompletions = new Dictionary<MissionType, int>();
    public Dictionary<MissionType, int> missionTypeFails = new Dictionary<MissionType, int>();
    public Dictionary<WeaponPlaystyle, int> weaponPlaystyleUsage = new Dictionary<WeaponPlaystyle, int>();
    public Dictionary<string, int> killDistances = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeKillDistances();
    }

    private void InitializeKillDistances()
    {
        killDistances["Short"] = 0;
        killDistances["Short to Medium"] = 0;
        killDistances["Medium"] = 0;
        killDistances["Medium to Long"] = 0;
        killDistances["Long"] = 0; //
    }

    public void RecordMissionCompletion(MissionType missionType)
    {
        if (!missionTypeCompletions.ContainsKey(missionType))
        {
            missionTypeCompletions[missionType] = 0;
        }
        missionTypeCompletions[missionType] += 1;
    }

    public int GetMissionTypeCompletions(MissionType missionType)
    {
        if (missionTypeCompletions.TryGetValue(missionType, out int completions))
        {
            return completions;
        }
        return 0;
    }

    public void RecordMissionFail(MissionType missionType)
    {
        if (!missionTypeFails.ContainsKey(missionType))
        {
            missionTypeFails[missionType] = 0;
        }
        missionTypeFails[missionType] += 1;
    }

    public int GetMissionTypeFails(MissionType missionType)
    {
        if (missionTypeFails.TryGetValue(missionType, out int fails))
        {
            return fails;
        }
        return 0;
    }

    public void RecordKill(WeaponPlaystyle playstyle, float distance)
    {
        // Track weapon playstyle usage
        if (!weaponPlaystyleUsage.ContainsKey(playstyle))
        {
            weaponPlaystyleUsage[playstyle] = 0;
        }
        weaponPlaystyleUsage[playstyle] += 1;

        // Determine and record kill distance
        string rangeKey = GetRangeKey(distance);
        killDistances[rangeKey] += 1;
        Debug.Log($"Weapon playstyle {playstyle} used for a kill at {rangeKey} range. Total kills with this playstyle: {weaponPlaystyleUsage[playstyle]}, Total kills at this range: {killDistances[rangeKey]}");
    }

    private string GetRangeKey(float distance)
    {
        if (distance <= 10)
        {
            return "Short";
        }
        else if (distance <= 20)
        {
            return "Short to Medium";
        }
        else if (distance <= 30)
        {
            return "Medium";
        }
        else if (distance <= 40)
        {
            return "Medium to Long";
        }
        else
        {
            return "Long";
        }
    }

    public void LogAllStats()
    {
        Debug.Log("Mission Completions:");
        foreach (var item in missionTypeCompletions)
        {
            Debug.Log($"Mission Type: {item.Key}, Completions: {item.Value}");
        }

        Debug.Log("Mission Fails:");
        foreach (var item in missionTypeFails)
        {
            Debug.Log($"Mission Type: {item.Key}, Fails: {item.Value}");
        }

        Debug.Log("Weapon Playstyle Usage:");
        foreach (var item in weaponPlaystyleUsage)
        {
            Debug.Log($"Playstyle: {item.Key}, Kills: {item.Value}");
        }

        Debug.Log("Kill Distances:");
        foreach (var item in killDistances)
        {
            Debug.Log($"Distance: {item.Key}, Kills: {item.Value}");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) 
        {
           LogAllStats();
        }
    }
}