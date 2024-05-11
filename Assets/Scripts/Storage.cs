/**
    * This class is used for storing data locally on the device.
    * This is useful for storing data that should be kept between sessions.
    *
    * Full documentation can be found at:
    * https://github.com/PontusAhlin/HorrorGame/wiki/Storage
    *
    * TODO:
    * - Refactor the class to use variables properties instead of normal getters and setters.
    *
    * Author(s): William Fridh
    */

using System;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Threading;

public class Storage: MonoBehaviour
{

    private const int amountOfAchievements = 5;
    private Data data;
    public static Storage Instance { get; private set; }
    private string filePath;
    private readonly string fileName = "storage.json";

    /**
        * Get Storage.
        *
        * This function checks the scene for a storage object and
        * returns it if it exists. If it does not exist, it will
        * create a new storage object and return it.
        *
        * This is the first function that should be called when
        * using the storage class.
        */
    public static Storage GetStorage()
    {
        Storage storage = GameObject.FindObjectOfType<Storage>();   // Find storage object.
        if (storage == null)                                        // If storage object does not exist.
        {
            //Debug.Log("Storage: No storage object found. Creating a new one.");
            if (Instance == null)
            {
                GameObject storageObject = new()                        // Create a new storage object.
                {
                    name = "StorageHolder"
                };
                storage = storageObject.AddComponent<Storage>();        // Add the storage script to the storage object.
                storage.transform.parent = null;                        // Set the storage object to the root of the scene.
                storage.filePath = Path.Combine(Application.persistentDataPath, storage.fileName); // Set the file path.
                storage.data = storage.GetData();                       // Get the data from the storage file.
            }
        }
        //storage.data ??= storage.GetData();
        return storage;                                             // Return the storage object.
    }

    /**
        * Awake Routine.
        *
        * This function is called when the script instance is being loaded.
        * It is used to make sure that only one instance of the storage
        * object exists at a time. Also known as "the singleton pattern".
        */
    void Awake()
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
    }

    /**
        * Get Data Routine.
        *
        * This function is used to get the data from the storage file and
        * sets the class objects data to a new data object with the data.
        * If the file does not exist, a new data object based on an empty
        * json string will be generated instead.
        *
        * TODO:
        * - Explore proper folder existance check and fixing.
        */
    private Data GetData()
    {
            // If the file does not exist, start a new storage.
            // Note that the files won't be created until Storage.Save() is called.
            if (!File.Exists(filePath))
            {
                Debug.Log("Storage: No storage file found. Creating a new one.");
                return new Data(amountOfAchievements);
            }
            // Read the file and return the data.
            string jsonData = File.ReadAllText(filePath);
            return new Data(amountOfAchievements, jsonData);
    }

    /**
        * Data Class.
        *
        * This class in defined as a nested class as it is only used by the storage
        * and should be communicated with through the storage class's getters,
        * setters, incremenetrs, et cetera.
        */
    private class Data
    {

        // Long-term data.
        public string username;
        public float musicVolume;
        public string[] highscore;
        public bool[] achievementsAchieved;
        public int[] achievementsProgress;

        // Stores the amount of viewers and likes for temporary use.
        public float lastGameViewers;
        public float lastGameLikes;

        /**
            * Constructor for the Data-class.
            */
        public Data(int amountOfAchievements, string jsonData = "{}")
        {
            JsonUtility.FromJsonOverwrite(jsonData, this);
            FixFaultyArrayLengths(amountOfAchievements);
        }

        /**
            * Fixes the length of the arrays if they are faulty.
            */
        private void FixFaultyArrayLengths(int amountOfAchievements)
        {
            if (achievementsAchieved == null || achievementsAchieved.Length != amountOfAchievements)
                Array.Resize(ref achievementsAchieved, amountOfAchievements);
            if (achievementsProgress == null || achievementsProgress.Length != amountOfAchievements)
                Array.Resize(ref achievementsProgress, amountOfAchievements);
            highscore ??= new string[0];
        }
    }

    /**
        * Save Data.
        *
        * Saves the data to the storage file.
        */
    private void SaveData()
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }

    // =============================== GETTERS ===============================
    public string GetUsername()
    {
        return data.username;
    }

    public string[] GetHighscore()
    {
        return data.highscore;
    }

    public float GetMusicVolume()
    {
        return data.musicVolume;
    }
    
    public float GetLastGameViewers()
    {
        return data.lastGameViewers;
    }

    public float GetLastGameLikes()
    {
        return data.lastGameLikes;
    }

    public bool GetAchievementAchieved(int index)
    {
        if (index < 0 || index >= data.achievementsAchieved.Length)
        {
            throw new ArgumentOutOfRangeException("Index out of range.");
        }
        return data.achievementsAchieved[index];
    }

    public int GetAchievementProgress(int index)
    {
        if (index < 0 || index >= data.achievementsProgress.Length)
        {
            throw new ArgumentOutOfRangeException("Index out of range.");
        }
        return data.achievementsProgress[index];
    }

    // =============================== SETTERS ===============================
    public void SetUsername(string value)
    {
        data.username = value;
        SaveData();
    }

    public void SetMusicVolume(float value)
    {
        data.musicVolume = value;
        SaveData();
    }

    public void SetLastGameViewers(float value)
    {
        data.lastGameViewers = value;
        SaveData();
    }

    public void SetLastGameLikes(float value)
    {
        data.lastGameLikes = value;
        SaveData();
    }

    public void SetAchievementArchieved(int index, bool value)
    {
        if (index < 0 || index >= data.achievementsAchieved.Length)
        {
            throw new ArgumentOutOfRangeException("Index out of range.");
        }
        data.achievementsAchieved[index] = value;
        SaveData();
    }

    public void SetAchievementProgress(int index, int value)
    {
        if (index < 0 || index >= data.achievementsProgress.Length)
        {
            throw new ArgumentOutOfRangeException("Index out of range.");
        }
        data.achievementsProgress[index] = value;
        SaveData();
    }

    // =============================== ADDERS ===============================

    /**
        * Adds a new highscore to the top five highscore list.
        * If the new highscore is not in the top five it will not be added.
        *
        * Returns true if the new highscore was added.
        */
    public bool AddToHighscore(string username, int score)
    {
        if (username == "" || score < 0)
        {
            throw new ArgumentException("Username or score is invalid.");
        }
        return AddToHighscore(username + ":" + score);
    }
    public bool AddToHighscore(string newHighscore)
    {
        // Get the top five highscores.
        string[] oldList = GetHighscore();
        // Create a new list with the new highscore. included.
        string[] newList = new string[oldList.Length + 1];
        for (int i = 0; i < oldList.Length; i++)
            newList[i] = oldList[i];
        newList[oldList.Length] = newHighscore;
        // Sort the list by score.
        Array.Sort(newList, (x, y) => int.Parse(y.Split(':')[1]).CompareTo(int.Parse(x.Split(':')[1])));
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
        // Save.
        data.highscore = newList;
        SaveData();
        // Check if new highscore was added.
        foreach (string score in newList)
            if (newHighscore.Contains(score))
                return true;
        return false;
    }
}