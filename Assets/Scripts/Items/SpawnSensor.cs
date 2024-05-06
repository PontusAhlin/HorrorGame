using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    *This script is attatched to a button which spawns in
    *the sensor. The sensor will not spawn if the max amount
    *has been reached.

    *Author(s): Sai Chintapalli
*/
public class SpawnSensor : MonoBehaviour{
    public GameObject Sensor;
    public GameObject Player;
    private GameObject temp;
    public static int maxAmount = 3;
    private int spawnedAmount = 0;

    // Update is called once per frame
    public void Spawn()
    {
        if(spawnedAmount < maxAmount){
            temp = Instantiate(Sensor, Player.transform.position, Quaternion.identity);
            spawnedAmount++;
            temp.name = "sensor " + spawnedAmount;
        }
    }
}
