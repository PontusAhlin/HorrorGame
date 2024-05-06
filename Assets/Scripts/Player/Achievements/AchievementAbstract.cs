/**
    * Achievement Abstract.
    *
    * This script is an interface for achievements.
    * It contains the basic structure of an achievement, with getters and setters.
    * It also contains a method to add Progress to the achievement.
    *
    * TODO:
    * - Add code to call the popup to notify the player.
    *
    * Author(s): William Fridh
    */
    
using UnityEngine;

public abstract class AchievementAbstract : MonoBehaviour
{

    protected int     Index;          // Index of the achievement (must be unique).
    protected string  Title;          // Title of the achievement.
    protected string  Description;    // Description of the achievement.
    protected bool    IsAchieved;     // If the achievement is archived or not.
    protected int     Progress;       // Progress of the achievement.
    protected int     MaxProgress;    // Maximum Progress of the achievement (Progress = MaxProgress -> archived).
    private string    SpritePath;     // Full sprite path.

    // Start is called before the first frame update
    void Start()
    {
        // Get status of achievement
        IsAchieved =  Storage.GetAchievementAchieved(0);
        Progress = Storage.GetAchievementProgress(0);
        // Check values.
        if (Title == null || Description == null || SpritePath == null)
        {
            throw new System.ArgumentNullException("Index, Title, Description, MaxProgress, or SpritePath is null.");
        }
    }

    // =============================== GETTERS ===============================
    public string GetTitle()
    {
        return Title;
    }
    public string GetDescription()
    {
        return Description;
    }
    public bool GetIsAchieved()
    {
        return IsAchieved;
    }
    public int GetProgress()
    {
        return Progress;
    }
    public int GetMaxProgress()
    {
        return MaxProgress;
    }
    public string GetSpritePath()
    {
        return SpritePath;
    }

    // =============================== SETTERS ===============================

    /**
        * Set the achievement as archived or not.
        * This can be used directly, but can also be called from SetProgress.
        */
    public void SetIsArchived(bool value)
    {
        IsAchieved = value;
        if (IsAchieved)
        {
            Progress = MaxProgress; // If archived, set Progress to max Progress.
            Storage.SetAchievementProgress(Index, MaxProgress);
            // Call the achievement popup.
        } else {
            Storage.SetAchievementArchieved(Index, value);
        }
    }

    /**
        * Set the Progress of the achievement.
        * If Progress is greater than max Progress, set as archived.
        * Else, set Progress to value.
        */
    public void SetProgress(int value)
    {
        if (Progress >= MaxProgress) // If Progress is greater than max Progress, set as archived.
        {
            SetIsArchived(true);
            Progress = MaxProgress;
            Storage.SetAchievementProgress(Index, MaxProgress);
        } else { // Else, set Progress to value.
            Progress = value;
            Storage.SetAchievementProgress(Index, value);
        }
    }

    /**
        * Set the sprite path of the achievement.
        * This is used to set the sprite of the achievement in a correct way.
        */
    public void SetSpritePath(string value)
    {
        SpritePath = value.Replace("Assets/", "").Replace("Resources/", ""); // Remove "Assets/" and "Resources/" from the path.;
    }

    // =============================== ADDERS ===============================

    /**
        * Add Progress to the achievement.
        * This is useful when the achievement is not binary, but has a Progress.
        */
    public void AddProgress(int Value)
    {
        Progress += Value;
        if (Progress >= MaxProgress) // If Progress is greater than max Progress, set as archived.
        {
            SetIsArchived(true);
            Progress = MaxProgress;
        }
        Storage.SetAchievementProgress(Index, Progress);
    }

}

