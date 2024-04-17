using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public GameObject doorOpened, doorClosed, inTrigger, outTrigger;
    bool DoorIsClosed = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Ball"){
        Debug.Log("door should open now");
        doorClosed.SetActive(false);
        doorOpened.SetActive(true);
        }
        Debug.Log("end or door opening bit");
    }

    private void OnTriggerExit(Collider other){
        Debug.Log("door should close");
        if(other.gameObject.tag == "Ball"){
        doorClosed.SetActive(true);
        doorOpened.SetActive(false);
        }
    }        
    // Update is called once per frame
    void Update()
    {
        
    }
}
