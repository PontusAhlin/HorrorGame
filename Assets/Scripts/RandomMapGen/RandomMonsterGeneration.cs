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
    [Tooltip("time delay between monsters spawned after the first")]
    public int timeSeconds = 3;
    [Tooltip("How many monsters should be generated in total")]
    public int MonsterAmount = 3;
    [Tooltip("how much to wait before the FIRST monster spawns")]
    public int firstMonsterDelay = 5;
    public int CurrentMonsterAmount = 0;
    [Tooltip("Insert the RandomMapHandler script here pls")]
    private int xCoord, zCoord;
    private bool firstMonsterSpawned = false;
    public RandomMapHandler RandomMapScript;
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
        if (firstMonsterSpawned)
            yield return new WaitForSeconds(timeSeconds);
        else
            {
                firstMonsterSpawned = true;
                yield return new WaitForSeconds(firstMonsterDelay);
            }
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
            if (monsterIndex == (MonsterPrefabs.Count-1))
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

