using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGeneration : MonoBehaviour
{
    public GameObject MonsterPrefab;
    [Tooltip("How frequent monsters should be generated")]
    public int timeSeconds = 3;
    [Tooltip("How many monsters should be generated")]
    public int MonsterAmount = 3;
    private int CurrentMonsterAmount = 0;
    void Start()
    {
        Instantiate(MonsterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        CurrentMonsterAmount++;
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
        Instantiate(MonsterPrefab, new Vector3(CurrentMonsterAmount*5, 0, 0), Quaternion.identity);
        CurrentMonsterAmount++;
        if(CurrentMonsterAmount < MonsterAmount){
            StartCoroutine(GenerateMonster());
        }
    }
}
