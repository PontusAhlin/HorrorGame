using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private Door Door;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Ball")){
            if (!Door.isOpen){
                Door.Open(other.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Ball")){
            if (!Door.isOpen){
                Door.Close();
            }
        }
    }
}
