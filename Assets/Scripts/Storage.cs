/**
    * This class is used for storing data locally on the device.
    * This is useful for storing data that should be kept between sessions.
    *
    * Full documentation can be found at:
    * https://github.com/PontusAhlin/HorrorGame/wiki/Storage
    *
    * Example #1:
    * - Storage.SetUsername("William");
    * - string username = Storage.GetUsername();
    *
    * Author(s): William Fridh
    */

using System.IO;
using UnityEngine;

public static class Storage
{

    // Static string used for setting up the file.
    private static string Path = Application.persistentDataPath + "/storage.json";

    // Used for debugging.
    public static string GetPath()
    {
        return Path;
    }

    /**
        * Checks if the storage file is valid.
        * This is important as the strcture might change troughout the development.
        *
        * A missing JSON-file or a JSON-file that does not match the StorageData-class
        * will be considered invalid.
        */
    private static bool StorageFileIsValid() {
        if (!File.Exists(Path)) return false;                                                   // Check if the file exists.
        string jsonDataFromFile     = File.ReadAllText(Path);                                   // Read from JSON-file.
        StorageData data            = JsonUtility.FromJson<StorageData>(jsonDataFromFile);      // Insert the JSON-data into a StorageData-object.
        string jsonDataFromObject   = JsonUtility.ToJson(data);                                 // Convert the StorageData-object back to JSON.
        return jsonDataFromFile != "" && jsonDataFromFile == jsonDataFromObject;                // Compare the two JSON-strings.
    }

    /**
        * Gets the data from the storage file.
        * If the file does not exist or is invalid a new file will be generated.
        */
    private static StorageData GetData()
    {

        if (!StorageFileIsValid())
        {
            Debug.LogWarning("Non-existing or invalid storage file. Generating a new one.");
            SaveData(new StorageData());
        }
        string jsonData = File.ReadAllText(Path);
        return JsonUtility.FromJson<StorageData>(jsonData);
    }

    /**
        * Saves the data to the storage file.
        */
    private static void SaveData(StorageData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(Path, jsonData);
    }

    // =============================== GETTERS ===============================
    public static string GetUsername()
    {
        StorageData data = GetData();
        return data.username;
    }

    public static string[] GetHighscore()
    {
        StorageData data = GetData();
        return data.highscore;
    }

    public static float GetMusicVolume()
    {
        StorageData data = GetData();
        return data.musicVolume;
    }
    
    public static float GetLastGameViewers()
    {
        StorageData data = GetData();
        return data.lastGameViewers;
    }

    public static float GetLastGameLikes()
    {
        StorageData data = GetData();
        return data.lastGameLikes;
    }

    public static bool GetAchievementAchieved(int index)
    {
        StorageData data = GetData();
        if (index < 0 || index >= data.achievementsAchieved.Length)
        {
            throw new System.ArgumentOutOfRangeException("Index out of range.");
        }
        return data.achievementsAchieved[index];
    }

    public static int GetAchievementProgress(int index)
    {
        StorageData data = GetData();
        if (index < 0 || index >= data.achievementsProgress.Length)
        {
            throw new System.ArgumentOutOfRangeException("Index out of range.");
        }
        return data.achievementsProgress[index];
    }

    // =============================== SETTERS ===============================
    public static void SetUsername(string value)
    {
        StorageData data = GetData();
        data.username = value;
        SaveData(data);
    }

    public static void SetMusicVolume(float value)
    {
        StorageData data = GetData();
        data.musicVolume = value;
        SaveData(data);
    }

    public static void SetLastGameViewers(float value)
    {
        StorageData data = GetData();
        data.lastGameViewers = value;
        SaveData(data);
    }

    public static void SetLastGameLikes(float value)
    {
        StorageData data = GetData();
        data.lastGameLikes = value;
        SaveData(data);
    }

    public static void SetAchievementArchieved(int index, bool value)
    {
        StorageData data = GetData();
        if (index < 0 || index >= data.achievementsAchieved.Length)
        {
            throw new System.ArgumentOutOfRangeException("Index out of range.");
        }
        data.achievementsAchieved[index] = value;
        SaveData(data);
    }

    public static void SetAchievementProgress(int index, int value)
    {
        StorageData data = GetData();
        if (index < 0 || index >= data.achievementsProgress.Length)
        {
            throw new System.ArgumentOutOfRangeException("Index out of range.");
        }
        data.achievementsProgress[index] = value;
        SaveData(data);
    }

    // =============================== ADDERS ===============================

    /**
        * Adds a new highscore to the top five highscore list.
        * If the new highscore is not in the top five it will not be added.
        *
        * Returns true if the new highscore was added.
        */
    public static bool AddToHighscore(string username, int score)
    {
        if (username == "" || score < 0)
        {
            throw new System.ArgumentException("Username or score is invalid.");
        }
        return AddToHighscore(username + ":" + score);
    }
    public static bool AddToHighscore(string newHighscore)
    {
        // Get the top five highscores.
        string[] oldList = GetHighscore();
        // Create a new list with the new highscore. included.
        string[] newList = new string[oldList.Length + 1];
        for (int i = 0; i < oldList.Length; i++)
            newList[i] = oldList[i];
        newList[oldList.Length] = newHighscore;
        // Sort the list by score.
        System.Array.Sort(newList, (x, y) => int.Parse(y.Split(':')[1]).CompareTo(int.Parse(x.Split(':')[1])));
        // Transfer the top five highscores to a new list.
        string[] topScores;
        if (newList.Length > 5)
        {
            topScores = new string[5];
            for (int i = 0; i < 5; i++)
                topScores[i] = newList[i];
        }
        else
        {
            topScores = newList;
        }
        // Load data.
        StorageData data = GetData();
        // Save.
        data.highscore = newList;
        SaveData(data);
        // Check if new highscore was added.
        foreach (string score in newList)
            if (newHighscore.Contains(score))
                return true;
        return false;
    }
}

public class StorageData
{
    // Long-term data.
    public string username;
    public float musicVolume;
    public string[] highscore = new string[0];
    public bool[] achievementsAchieved = new bool[5];
    public int[] achievementsProgress = new int[5];

    // Stores the amount of viewers and likes for temporary use.
    public float lastGameViewers;
    public float lastGameLikes;
}