/**
    * Achievement Abstract.
    *
    * This script is an interface for achievements.
    * It contains the basic structure of an achievement, with getters and setters.
    * It also contains a method to add progress to the achievement.
    *
    * TODO:
    * - Add code to call the popup to notify the player.
    * - Refactor into having variable properties.
    *
    * Author(s): William Fridh
    */
    
using UnityEngine;

public abstract class AchievementAbstract : MonoBehaviour
{

    // =============================== VARIABLES ===============================
    protected int     index ;          // index of the achievement (must be unique).
    protected string  title;          // title of the achievement.
    protected string  description;    // description of the achievement.
    protected bool    isAchieved;     // If the achievement is archived or not.
    protected int     progress;       // progress of the achievement.
    protected int     maxProgress;    // Maximum progress of the achievement (progress = maxProgress -> archived).
    private string    spritePath;     // Full sprite path.
    private Storage   storage;        // Storage object.

    // =============================== INITIALIZATION ===============================
    void Awake()
    {
        // Check Values.
        if (title == null || description == null || spritePath == null)
            throw new System.ArgumentNullException("index, title, description, maxProgress, or spritePath is null.");
        // Get storage object.
        storage = Storage.GetStorage();
        // Get status of achievement
        isAchieved =  storage.GetAchievementAchieved(index);
        progress = storage.GetAchievementProgress(index);
    }

    // =============================== GETTERS ===============================
    public string GetTitle()
    {
        return title;
    }
    public string GetDescription()
    {
        return description;
    }
    public bool GetIsAchieved()
    {
        return isAchieved;
    }
    public int GetProgress()
    {
        return progress;
    }
    public int GetMaxProgress()
    {
        return maxProgress;
    }
    public string GetSpritePath()
    {
        return spritePath;
    }

    // =============================== SETTERS ===============================

    /**
        * Set the achievement as archived or not.
        * This can be used directly, but can also be called from Setprogress.
        */
    public void SetIsArchived(bool Value)
    {
        isAchieved = Value;
        if (isAchieved)
        {
            progress = maxProgress; // If archived, set progress to max progress.
            storage.SetAchievementProgress(index, maxProgress);
            // Call the achievement popup.
        }
        storage.SetAchievementArchieved(index, Value);
        if (Value)
            Debug.Log("Achievement " + GetTitle() + " is archived.");
    }

    /**
        * Set the progress of the achievement.
        * If progress is greater than max progress, set as archived.
        * Else, set progress to Value.
        *
        * Returns a boolean if the achievement is archived or not.
        */
    public bool SetProgress(int Value)
    {
        if (progress >= maxProgress) // If progress is greater than max progress, set as archived.
        {
            SetIsArchived(true);
            progress = maxProgress;
            storage.SetAchievementProgress(index, maxProgress);
        } else { // Else, set progress to Value.
            progress = Value;
            storage.SetAchievementProgress(index, Value);
        }
        return isAchieved;
    }

    /**
        * Set the sprite path of the achievement.
        * This is used to set the sprite of the achievement in a correct way.
        */
    public void SetSpritePath(string Value)
    {
        // Remove "Assets/" and "Resources/" from the path.;
        spritePath = Value.Replace("Assets/", "").Replace("Resources/", "");
    }

    // =============================== ADDERS ===============================

    /**
        * Add progress to the achievement.
        * This is useful when the achievement is not binary, but has a progress.
        *
        * Returns a boolean if the achievement is archived or not.
        */
    public bool AddProgress(int Value)
    {
        if (isAchieved) // If the achievement is already archived, return.
            return true;
        progress += Value;
        if (progress >= maxProgress) // If progress is greater than max progress, set as archived.
        {
            SetIsArchived(true);
            progress = maxProgress;
        }
        storage.SetAchievementProgress(index, progress);
        return isAchieved;
    }

}

