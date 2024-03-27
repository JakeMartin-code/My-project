using System;
using UnityEngine;

public interface IMissionProgress
{
   // float CurrentProgress { get; }
    bool UpdateProgress();
    bool IsComplete();
    void ResetProgress();
}

public class KillProgress : IMissionProgress
{
    private int killGoal;
    private int currentKills = 0; // Tracks the number of kills achieved towards the goal.

    // Constructor to set the initial kill goal.
    public KillProgress(int killAmount)
    {
        killGoal = killAmount;
    }

    // Property to get the current progress as a percentage.
    public float CurrentProgress
    {
        get { return (float)currentKills / killGoal; }
    }

    // Method to update the progress, assuming each call represents one kill.
    // This method does not take parameters as per your requirements.
    public bool UpdateProgress()
    {
        if (currentKills < killGoal)
        {
            currentKills++;
            return IsComplete(); // Check if the mission is complete after updating the kill count.
        }
        return false; // Indicates no update was needed (already complete or an error occurred).
    }

    // Checks if the kill goal has been reached.
    public bool IsComplete()
    {
        return currentKills >= killGoal;
    }

    // Resets the progress, allowing the mission to start over.
    public void ResetProgress()
    {
        currentKills = 0;
    }

    // Provides the remaining kills needed to complete the mission.
    public int RemainingKills()
    {
        return killGoal - currentKills;
    }
}

public class InteractionProgress : IMissionProgress
{
    private GameObject interactObject;
    private bool interacted;

    public float CurrentProgress => interacted ? 1 : 0;

    public bool UpdateProgress()
    {
        interacted = true;
        return IsComplete();
    }

    public bool IsComplete()
    {
        return interacted;
    }

    public void ResetProgress()
    {
        interacted = false;
    }
}

public class SurviveProgress : IMissionProgress
{
    private float surviveTime; // Total time to survive
    private float remainingTime; // Remaining time to survive

    public float CurrentProgress => 1 - (remainingTime / surviveTime);

    public SurviveProgress(float survivalTime)
    {
        surviveTime = survivalTime;
        remainingTime = survivalTime; // Initialize remaining time
    }

    public bool UpdateProgress(float deltaTime)
    {
        if (remainingTime > 0)
        {
            remainingTime -= deltaTime;
            if (remainingTime < 0)
            {
                remainingTime = 0; // Prevents negative values
            }
        }
        return IsComplete();
    }

    public bool IsComplete() => remainingTime <= 0;

    public void ResetProgress() => remainingTime = surviveTime;

    // This method returns the remaining time in MM:SS format.
    public string RemainingTimeFormatted()
    {
        int minutes = (int)remainingTime / 60;
        int seconds = (int)remainingTime % 60;
        return string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    // Implement the interface's parameterless UpdateProgress method.
    public bool UpdateProgress()
    {
        // This method needs to be here to comply with the interface, but it might not be used.
        // You can leave it empty or throw a NotImplementedException,
        // depending on how you plan to use the SurviveProgress class.
        throw new NotImplementedException("Use UpdateProgress(float deltaTime) for SurviveProgress.");
    }
}


