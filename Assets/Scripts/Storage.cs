/**
    * This class is used for storing data locally on the device.
    * This is useful for storing data that should be kept between sessions.
    *
    * Example #1:
    * - Storage.SetUsername("William");
    * - string username = Storage.GetUsername();
    *
    * Author(s): William Fridh
    */

using System.IO;
using UnityEngine;

public class Storage
{

    // Static string used for setting up the file.
    private static string path = Application.persistentDataPath + "/storage.json";

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
    private static StorageData getData()
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
        StorageData data = getData();
        return data.username;
    }

    // =============================== SETTERS ===============================
    public static void SetUsername(string value)
    {
        StorageData data = getData();
        data.username = value;
        SaveData(data);
    }
}

public class StorageData
{
    public string username;
}