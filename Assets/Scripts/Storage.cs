/**
    * This class is used for storing data locally on the device.
    * This is useful for storing data that should be kept between sessions.
    *
    * Full documentation can be found at:
    * https://github.com/PontusAhlin/HorrorGame/wiki/Storage
    *
    * Example #1:
    * - storage.SetUsername("William");
    * - string username = Storage.GetUsername();
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

    // Settings.
    private const int amountOfAchievements = 5;

    // Empty variables.
    private Data data;
    public static Storage instance;

    /**
        * Define Storage Path.
        *
        * As diffrent operating systems have diffrent ways of storing files
        * we need to take this into consideration. This is done by defining
        * the path to the storage file in different ways depending on the OS.
        */
    private string filePath;
    private const string fileName = "storage.json";

    #if UNITY_IPHONE
    private string fileFolder = Path.Combine(Application.dataPath, "Raw");
    #elif UNITY_ANDROID
    private string fileFolder = Path.Combine("jar:file://", Application.dataPath, "assets"); // Changed from "!/assets/".
    #else
    // Mac, Windows, and Linux
    private string fileFolder = Path.Combine(Application.dataPath, "StreamingAssets");
    #endif

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




    #if UNITY_IPHONE
    Debug.Log(1);
    #elif UNITY_ANDROID
    Debug.Log(2);
    //Detects this...
    #else
    Debug.Log(3);
    #endif


        Storage storage = GameObject.FindObjectOfType<Storage>();   // Find storage.
        if (storage == null)                                        // If storage object does not exist.
        {
            Debug.Log("Storage: No storage object found. Creating a new one.");
            GameObject storageObject = new GameObject();            // Create a new storage object.
            storageObject.name = "Storage";                         // Set the name of the storage object.
            storage = storageObject.AddComponent<Storage>();        // Add the storage script to the storage object.
            storage.transform.parent = null;                        // Set the storage object to the root of the scene.
        }
        return storage;                                             // Return the storage object.
    }

    /**
        * Upon Awakening.
        *
        * Upon awake we need to combine the file folder and name
        * to create the final path. Then we start a coroutine to
        * get the data from the file located at the final file path.
        */

    void Awake()
    {
        // Construct full file path.
        filePath = Path.Combine(fileFolder, fileName);
        // Create a coroutine to get the data.
        StartCoroutine(GetData());
    }

    /**
        * Get Data Routine.
        *
        * This function is used to get the data from the storage file and
        * sets the class objects data to a new data object with the data.
        * If the file does not exist, a new data object based on an empty
        * json string will be generated instead.
        */
    private IEnumerator GetData()
    {
        string dataString = "";                                         // Create a string to store the data.
        if (filePath.Contains("://") || filePath.Contains(":///"))      // Check if the file path is a URL (mobile devices). 
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);        // Create a new web request.
            yield return www.SendWebRequest();                          // Send the web request.
            if (                                                        // Catch failed fetch.
                www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError
            ) {
                Debug.Log("Storage: File does not exist. Starting a new one (mobile).");
                dataString = "{}";                                      // Create a new empty json string.
            }
            else {
                dataString = www.downloadHandler.text;                  // Store resulting data.
            }
        }
        else                                                            // If the file path is not a URL (PC).
        {
            if (!File.Exists(filePath))                                 // If the file does not exist.
            {
                Debug.Log("Storage: File does not exist. Starting a new one (PC).");
                dataString = "{}";                                      // Create a new empty json string.
            }
            dataString = File.ReadAllText(filePath);                    // Read the existing file.
        }
        data = new Data(amountOfAchievements, dataString);              // Create a new data object.
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
            FixFaultyArrayLengths();
        }

        /**
            * Fixes the length of the arrays if they are faulty.
            */
        private void FixFaultyArrayLengths()
        {
            if (achievementsAchieved == null || achievementsAchieved.Length != amountOfAchievements)
                Array.Resize(ref achievementsAchieved, amountOfAchievements);
            if (achievementsProgress == null || achievementsProgress.Length != amountOfAchievements)
                Array.Resize(ref achievementsProgress, amountOfAchievements);
            if (highscore == null)
                highscore = new string[0];
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
            throw new System.ArgumentOutOfRangeException("Index out of range.");
        }
        return data.achievementsAchieved[index];
    }

    public int GetAchievementProgress(int index)
    {
        if (index < 0 || index >= data.achievementsProgress.Length)
        {
            throw new System.ArgumentOutOfRangeException("Index out of range.");
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
            throw new System.ArgumentOutOfRangeException("Index out of range.");
        }
        data.achievementsAchieved[index] = value;
        SaveData();
    }

    public void SetAchievementProgress(int index, int value)
    {
        if (index < 0 || index >= data.achievementsProgress.Length)
        {
            throw new System.ArgumentOutOfRangeException("Index out of range.");
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
            throw new System.ArgumentException("Username or score is invalid.");
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