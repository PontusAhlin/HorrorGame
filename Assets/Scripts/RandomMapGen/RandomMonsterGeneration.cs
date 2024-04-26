using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMonsterGeneration : MonoBehaviour
{
    public GameObject MonsterPrefab;
    [Tooltip("How frequent monsters should be generated")]
    public int timeSeconds = 3;
    [Tooltip("How many monsters should be generated")]
    public int MonsterAmount = 3;
    private int CurrentMonsterAmount = 0;
    [Tooltip("Insert the RandomMapHandler script here pls")]
    public RandomMapHandler randscript;
    int mapOffsetX = 0;
    int mapOffsetZ = 0;
    [SerializeField]
    int xCoord, zCoord;
    void Start()
    {
        if(CurrentMonsterAmount < MonsterAmount){
            StartCoroutine(GenerateMonster());
        }
    }

    IEnumerator GenerateMonster(){
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(timeSeconds);
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        xCoord = Random.Range(0, gameObject.GetComponent<RandomMapHandler>().MapWidth);
        zCoord = Random.Range(0, gameObject.GetComponent<RandomMapHandler>().MapHeight);
        while(randscript.gridHandler[xCoord,zCoord] == RandomMapHandler.Grid.EMPTY &&
        !(xCoord < gameObject.GetComponent<RandomMapHandler>().MapWidth/2 + 1 && xCoord > gameObject.GetComponent<RandomMapHandler>().MapWidth/2 - 1 &&
        zCoord < gameObject.GetComponent<RandomMapHandler>().MapHeight/2 + 1 && zCoord > gameObject.GetComponent<RandomMapHandler>().MapHeight/2 - 1)){
            xCoord = Random.Range(0, gameObject.GetComponent<RandomMapHandler>().MapWidth);
            zCoord = Random.Range(0, gameObject.GetComponent<RandomMapHandler>().MapHeight);
        }
        xCoord = xCoord * gameObject.GetComponent<RandomMapHandler>().RoomSize;
        zCoord = zCoord * gameObject.GetComponent<RandomMapHandler>().RoomSize;
        Instantiate(MonsterPrefab, new Vector3(xCoord, 2, zCoord), Quaternion.identity);
        CurrentMonsterAmount++;
        if(CurrentMonsterAmount < MonsterAmount){
            StartCoroutine(GenerateMonster());
        }
    }
}
