using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private Door Door;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("door should open");
        if(other.CompareTag("Ball") || other.CompareTag("Player") || other.CompareTag("Monster")){
        Debug.Log("door should open 2");
            if (!Door.isOpen){
                Debug.Log("should trigger");
                Door.Open(other.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Ball") || other.CompareTag("Player") || other.CompareTag("Monster")){
            if (Door.isOpen){
                Debug.Log("should also trigger");
                Door.Close();
            }
        }
    }
}
