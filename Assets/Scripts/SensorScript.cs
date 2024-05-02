using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    *This code is is used on the Sensor prefab. It detects
    *a monster within its range and will send a "notification"
    *to the player with its respective frequency.
    Author(s): Sai Chintapalli
*/
public class SensorScript : MonoBehaviour
{
    public GameObject charger;
    public GameObject man;
    public GameObject ghost;
    public int chargerFreq = 10;
    public int manFreq = 5;
    public int ghostFreq = 20;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Monster")){
            if(other.name.Contains(charger.name)){
                Debug.Log("ALERT: Frequency spike of " + chargerFreq);
            }
            else if(other.name.Contains(man.name)){
                Debug.Log("ALERT: Frequency spike of " + manFreq);
            }
            else if(other.name.Contains(ghost.name)){
                Debug.Log("ALERT: Frequency spike of " + ghostFreq);
            }
            else{
                Debug.Log("this message should not be seen");
            }
        }
    }
}
