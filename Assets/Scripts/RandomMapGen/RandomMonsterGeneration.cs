/**
    * 
    *
    *
    * Authors: Sai Chintapalli, Pontus Åhlin
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMonsterGeneration : MonoBehaviour
{
    public GameObject Player;
    [Tooltip("TRUE = spawn one of each monster first before randomizing. FALSE = truly random spawns")]
    public bool SpawnOneOfEach;
    [Tooltip("monsters in this list will be randomly picked to spawn")]
    public List<GameObject> MonsterPrefabs = new List<GameObject>();
    [Tooltip("How frequent monsters should be generated")]
    public int timeSeconds = 3;
    [Tooltip("How many monsters should be generated")]
    public int MonsterAmount = 3;
    public int CurrentMonsterAmount = 0;
    [Tooltip("Insert the RandomMapHandler script here pls")]
    public RandomMapHandler RandomMapScript;
    [SerializeField]
    int xCoord, zCoord;
    [SerializeField]
    //private int safeArea = 0;

    //Reference to access BoxCast
    public BoxCast boxCast;


    void Start()
    {

        if(CurrentMonsterAmount < MonsterAmount){
            StartCoroutine(GenerateMonster(0)); 
        }
    }

    IEnumerator GenerateMonster(int monsterIndex){
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield   return new WaitForSeconds(timeSeconds);
        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        xCoord = Random.Range(0, gameObject.GetComponent<RandomMapHandler>().MapWidth - 1);
        zCoord = Random.Range(0, gameObject.GetComponent<RandomMapHandler>().MapHeight - 1);
        while(RandomMapScript.gridHandler[xCoord,zCoord] == RandomMapHandler.Grid.EMPTY ||
        (xCoord == Mathf.RoundToInt(Player.transform.position.x/gameObject.GetComponent<RandomMapHandler>().RoomSize)&& 
        zCoord == Mathf.RoundToInt(Player.transform.position.z/gameObject.GetComponent<RandomMapHandler>().RoomSize))){
            xCoord = Random.Range(0, gameObject.GetComponent<RandomMapHandler>().MapWidth);
            zCoord = Random.Range(0, gameObject.GetComponent<RandomMapHandler>().MapHeight);
        }
        xCoord = xCoord * gameObject.GetComponent<RandomMapHandler>().RoomSize;
        zCoord = zCoord * gameObject.GetComponent<RandomMapHandler>().RoomSize;

        if (SpawnOneOfEach)
        {
            //Debug.Log(monsterIndex);
            if (monsterIndex == MonsterPrefabs.Count)
                SpawnOneOfEach = false;
            Instantiate(MonsterPrefabs[monsterIndex], new Vector3(xCoord, 2, zCoord), Quaternion.identity);
            CurrentMonsterAmount++;
            if(CurrentMonsterAmount < MonsterAmount)
            {
                StartCoroutine(GenerateMonster(monsterIndex+1));
            
            }
        }
        else
        {
            Instantiate(MonsterPrefabs[UnityEngine.Random.Range(0,MonsterPrefabs.Count)], new Vector3(xCoord, 2, zCoord), Quaternion.identity);
            CurrentMonsterAmount++;
            if(CurrentMonsterAmount < MonsterAmount)
            {
                StartCoroutine(GenerateMonster(0));
            }
        }
    }
}

