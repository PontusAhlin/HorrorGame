using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public GameObject doorOpenedOut, doorOpenedIn, doorClosed, inTrigger, outTrigger;
    bool DoorIsClosed = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Ball")){
                //if(gameObject == inTrigger){
                    Debug.Log(gameObject.name);
                    Debug.Log("door opens in");
                    doorClosed.SetActive(false);
                    doorOpenedOut.SetActive(true);
                //}
            Debug.Log("door should open now");
        }
        Debug.Log("end or door opening bit");
    }

    private void OnTriggerExit(Collider other){
        Debug.Log("door should close");
        if(other.gameObject.tag == "Ball"){
        doorClosed.SetActive(true);
        doorOpenedOut.SetActive(false);
        }
    }        
    // Update is called once per frame
    void Update()
    {
        
    }
}
