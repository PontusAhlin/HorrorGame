using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorBehavior1 : MonoBehaviour
{
    public GameObject doorTrigger, openDoor, closedDoor;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("MainCamera")){
            if(closedDoor.activeInHierarchy == true){
                    Debug.Log("door opens");
                    closedDoor.SetActive(false);
                    openDoor.SetActive(true);
                }
            Debug.Log("door should open now");
        }
        Debug.Log("end or door opening bit");
    }

    private void OnTriggerExit(Collider other){
        Debug.Log("door should close");
        if(other.gameObject.tag == "MainCamera"){
            closedDoor.SetActive(true);
            openDoor.SetActive(false);
        }
    }        
    // Update is called once per frame
    void Update()
    {
        
    }
}
