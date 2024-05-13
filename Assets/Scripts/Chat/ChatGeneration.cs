/**
    * This script has a function called GenerateMessage which returns one [1] string
    * containing a thing that our chat would be saying at this point in time, based
    * on some numbers corresponding to what the player is doing right now.
    *
    * Ideally GenerateMessage should be called at different speeds, faster for when
    * the monster is around and much slower when nothing is happening.
    *
    * It now make suse of a nested class to hold the chat messages loaded form a JSON
    * file. The JSON file is loaded in the Awake function.
    *
    * Author(s): Alin, Pontus Åhlin, William Fridh
    */

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Random = UnityEngine.Random;
using System.Collections;

public class ChatGeneration : MonoBehaviour
{
    [Tooltip("this is the chance that a regular message will be generated by GenerateMessage()")]
    public float RegularChance = 0.5f;

    [Tooltip("this is the chance that a scared/excited message will be generated by GenerateMessage()")]
    public float MonsterChance = 0.25f;

    [Tooltip("this is the chance that a monster request will be generated by GenerateMessage()")]
    public float RequestChance = 0.25f;

    [Tooltip("this is the monster GameObject whose EXACT NAME IN UNITY will be requested")]
    public GameObject MonsterObject;

    [Tooltip("Path to the chat messages json file. Default is \"/Resources/ChatMessages.json\".")]
    [SerializeField] string chatMessagesPath = "/Resources/ChatMessages.json";

    private ChatMessages chatMessages;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        string path = Application.dataPath + "/" + chatMessagesPath;

        // If the application is running on Android, add "jar:file://" at the beginning of the path.
        if (Application.platform == RuntimePlatform.Android)
            path = "jar:file://" + path;
        StartCoroutine("LoadData", path);
    }





IEnumerator LoadData (string path)
{
string jsonData;
if (path.Contains ("://") || path.Contains (":///")) 
{
UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(path);
yield return www.SendWebRequest();
jsonData = www.downloadHandler.text;
}
else 
{
jsonData = File.ReadAllText (path);
}
        chatMessages = JsonUtility.FromJson<ChatMessages>(jsonData);
}










    // Nested class for holding messages.
    class ChatMessages
    {
        public List<string> RegularMessages;
        public List<string> MonsterMessages;
        public List<string> RequestMessages;
    }


    /*  The messages are sorted into three categories: Positive, Negative and request. 
        The negative comments will be shown when no monster is seen for a while/low viewers.
        The positive comments will be shown when you record a monster and have higher viewers.
        The request comments will be shown when the viewers wants to see a before seen monster. 
    */
    public string GenerateMessage(string MessageType)
    {
        string message = null;

        //float pickedMessagePointer = UnityEngine.Random.value;

        /*
        HOW THIS WORKS:
        so we have two chances in play rn, request & monster chance
        request chance is the chance the message will be a request message (ex: 0.20f)
        monster chance is the chance the message will be a scared message (ex: 0.30f)
        then we pick a number between 0 and 1.00f

        monsterchance has priority and requests have lower priority, so if monster chance is 1.0f, and request is 0.2f
        it'll always be a monster message

        so if the number is 
        under 0.20f, monster spook message
        under 0.20f+0.30f, we get a request
        if it's over 0.20f+0.30f, we get a regular random chat message
        */

        /*
        if (pickedMessagePointer < MonsterChance)
            message = MonsterMessages[UnityEngine.Random.Range(0,MonsterMessages.Count)];
        else if (pickedMessagePointer < (MonsterChance + RequestChance))
            message = RequestMessages[UnityEngine.Random.Range(0,RequestMessages.Count)];
        else 
            message = RegularMessages[UnityEngine.Random.Range(0,RegularMessages.Count)];
        */


        switch(MessageType){

            case "Positive":
                message = chatMessages.MonsterMessages[Random.Range(0,chatMessages.MonsterMessages.Count)];
                break;    

            case "Negative":
                message = chatMessages.RegularMessages[Random.Range(0,chatMessages.RegularMessages.Count)];
                break;

            case "Request":
                message = chatMessages.RequestMessages[Random.Range(0,chatMessages.RequestMessages.Count)];
                break;
        }

        return message;
    }
}
