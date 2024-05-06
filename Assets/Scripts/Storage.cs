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
    private static string path = Application.persistentDataPath + "/storage.json";

    // Used for debugging.
    public static string GetPath()
    {
        return path;
    }

    /**
        * Checks if the storage file is valid.
        * This is important as the strcture might change troughout the development.
        */
    private static bool StorageFileIsValid() {
        string jsonDataFromFile = File.ReadAllText(path);
        StorageData data = JsonUtility.FromJson<StorageData>(jsonDataFromFile);
        string jsonDataFromObject = JsonUtility.ToJson(data);
        return jsonDataFromFile == jsonDataFromObject;
    }

    /**
        * Gets the data from the storage file.
        * If the file does not exist or is invalid a new file will be generated.
        */
    private static StorageData GetData()
    {

        if (!File.Exists(path) || !StorageFileIsValid())
        {
            Debug.LogWarning("Non-existing or invalid storage file. Generating a new one.");
            SaveData(new StorageData());
        }
        string jsonData = File.ReadAllText(path);
        return JsonUtility.FromJson<StorageData>(jsonData);
    }

    /**
        * Saves the data to the storage file.
        */
    private static void SaveData(StorageData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(path, jsonData);
    }

    // =============================== GETTERS ===============================
    public static string GetUsername()
    {
        StorageData data = GetData();
        return data.username;
    }

    public static string[] GetTopFiveHighscore()
    {
        StorageData data = GetData();
        return data.topFiveHighscore;
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
    public static bool AddToTopFiveHighscore(string username, int score)
    {
        return AddToTopFiveHighscore(username + ":" + score);
    }
    public static bool AddToTopFiveHighscore(string newHighscore)
    {
        bool newHighscoreAdded = false;
        // Seperate given data.
        int newScore = int.Parse(newHighscore.Split(':')[1]);
        // Load data.
        StorageData data = GetData();
        // Create a new array and clone content.
        int newArrLength = data.topFiveHighscore.Length + 1;
        if (newArrLength > 5)
        {
            newArrLength = 5;
        }
        string[] newArray = new string[newArrLength];
        if (newArrLength == 1)
        {
            newArray[0] = newHighscore;
        }
        else
        {
            int i = 0;
            for (int ii = 0; ii < newArrLength; ii++)
            {
                if (int.Parse(data.topFiveHighscore[i].Split(':')[1]) < newScore && !newHighscoreAdded)
                {
                    newArray[ii] = newHighscore;
                    newHighscoreAdded = true;
                }
                else
                {
                    newArray[ii] = data.topFiveHighscore[i++];
                }
            }
        }
        // Save.
        data.topFiveHighscore = newArray;
        SaveData(data);
        return newHighscoreAdded;
    }
}

public class StorageData
{
    // Long-term data.
    public string username;
    public float musicVolume;
    public string[] topFiveHighscore = new string[0];
    public bool[] achievementsAchieved = new bool[2];
    public int[] achievementsProgress = new int[2];

    // Stores the amount of viewers and likes for temporary use.
    public float lastGameViewers;
    public float lastGameLikes;
}