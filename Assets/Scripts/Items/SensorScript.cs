using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    *This code is is used on the Sensor prefab. It detects
    *a monster within its range and will send a "notification"
    *to the player with its respective frequency.

    *Author(s): Sai Chintapalli
*/
public class SensorScript : MonoBehaviour
{
    public GameObject charger;
    public GameObject man;
    public GameObject ghost;
    public string chargerName = "charger";
    public string mannequinName = "mannequin";
    public string ghostName = "ghost";
    public GameObject Antenna1;
    public GameObject Antenna2;
    private string message;
    [SerializeField]
    private GameObject InterfaceObject;
    private InGameInterface InterfaceScript;


    private void Start() {
        InterfaceObject = GameObject.Find("In Game Interface");
        InterfaceScript = InterfaceObject.GetComponent<InGameInterface>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Monster")){
            if(other.name.Contains(charger.name)){
                message = "Detected a " + chargerName + " nearby!";
            }
            else if(other.name.Contains(man.name)){
                message = "Detected a " + mannequinName + " nearby!";
            }
            else if(other.name.Contains(ghost.name)){
                message = "Detected a " + ghostName + " nearby!";
            }
            else{
                message = "ERROR: UNKOWN MONSTER";
            }
            message = "(" + this.name + ") " + "ALERT:" + "\n" + message;
            Debug.Log(message);
            InterfaceScript.SendNotification(message, "Alert");   
        }
    }

    void Update() {
        Antenna1.transform.Rotate(0,0,1);
        Antenna2.transform.Rotate(0,0,-1);
    }
}
