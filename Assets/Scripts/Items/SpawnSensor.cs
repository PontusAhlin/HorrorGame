using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSensor : MonoBehaviour
{
    public GameObject Sensor;
    public GameObject Player;


    // Update is called once per frame
    public void Spawn()
    {
        Instantiate(Sensor, Player.transform.position, Quaternion.identity);
    }
}
