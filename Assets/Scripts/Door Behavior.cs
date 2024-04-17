using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public GameObject doorOpened, doorClosed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Ball"){
        doorClosed.SetActive(false);
        doorOpened.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision other) {
        doorClosed.SetActive(true);
        doorOpened.SetActive(false);
    }        
    // Update is called once per frame
    void Update()
    {
        
    }
}
