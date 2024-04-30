using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlRoomButton : MonoBehaviour
{
    public AudioSource pressedSound;    
    public AudioSource releasedSound;

    void OnTriggerEnter(){
        pressedSound.pitch = 1;
        pressedSound.Play();
        Debug.Log("trigger entered");
        GameObject EscapeDoorTrigger = GameObject.Find("/justDoor(withCam)(Clone)/EscapeDoorTrigger");
        Debug.Log(EscapeDoorTrigger);
        EscapeDoorTrigger.SetActive(true);
        //onPressed.Invoke();


    }

    void OnTriggerExit(){
        releasedSound.pitch = Random.Range(1.1f, 1.2f);
        releasedSound.Play();
        //onReleased.Invoke();
    }
}