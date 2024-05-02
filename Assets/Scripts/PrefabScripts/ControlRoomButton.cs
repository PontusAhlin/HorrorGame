using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlRoomButton : MonoBehaviour
{
    public AudioSource pressedSound;    
    public AudioSource releasedSound;
    public Material onMaterial;

    void OnTriggerEnter(Collider other){
        if (other.tag == "Player")
        {
            pressedSound.pitch = 1;
            pressedSound.Play();
            //Debug.Log("trigger entered");
            GameObject EscapeDoorTrigger = GameObject.Find("/justDoor(withCam)(Clone)/EscapeDoorTrigger");
            GameObject LockedDoor = GameObject.Find("justDoor(withCam)(Clone)/LockedDoor");
            GameObject Bulb = GameObject.Find("1x1ControlRoomFloor(Clone)/ControlPanel/Bulb");
            Debug.Log(Bulb);
            EscapeDoorTrigger.GetComponent<Collider>().isTrigger = true;
            LockedDoor.SetActive(false);
            Bulb.GetComponent<MeshRenderer>().material = onMaterial;
            //onPressed.Invoke();
        }

    }

    void OnTriggerExit(Collider other){
        if (other.tag == "Player")
        {
            releasedSound.pitch = Random.Range(1.1f, 1.2f);
            releasedSound.Play();
            //onReleased.Invoke();
        }
    }
}