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
    public RandomMapHandler randscript;
    int mapOffsetX;
    int mapOffsetZ;
    [SerializeField]
    float xCoord, zCoord;
    void Start()
    {
        mapOffsetX = -randscript.RoomSize;
        mapOffsetZ = randscript.RoomSize;

        if(CurrentMonsterAmount < MonsterAmount){
            StartCoroutine(GenerateMonster());
        }
        //if (randscript.gridHandler[i][j] == RandomMapHandler.Grid.EMPTY);
    }

    IEnumerator GenerateMonster(){
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(timeSeconds);
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        xCoord = Random.Range(0, randscript.RoomSize*randscript.MapWidth);
        zCoord = Random.Range(0, randscript.RoomSize*randscript.MapWidth);
        Instantiate(MonsterPrefab, new Vector3(xCoord + mapOffsetX, 1, zCoord + mapOffsetZ), Quaternion.identity);
        CurrentMonsterAmount++;
        if(CurrentMonsterAmount < MonsterAmount){
            StartCoroutine(GenerateMonster());
        }
    }
}
