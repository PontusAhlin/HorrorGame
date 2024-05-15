using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
/*
    *When the notification is clicked, it should set the
    *its respective sensor to active and have it point towards it.

    *Author(s): Sai Chintapalli
*/
public class NotifcationScript : MonoBehaviour
{
    public GameObject sensor;
    public GameObject pointerWrapper;

    public CustomAnimations customAnimations;

    private PointerScript pointerScript;

    // Start is called before the first frame update
    void Start(){
        pointerWrapper = PointerScript.pointerWrapper;
        pointerScript = pointerWrapper.GetComponent<PointerScript>();

		string notifMessage = this.transform.Find("Text Wrapper").Find("Text")
        .gameObject.GetComponent<TMPro.TextMeshProUGUI>().text;

        if(notifMessage.Contains("test sensor")){
            sensor = GameObject.Find("test sensor");
        }
        if(notifMessage.Contains("sensor 1")){
            sensor = GameObject.Find("sensor 1");
        }
        else if(notifMessage.Contains("sensor 2")){
            sensor = GameObject.Find("sensor 2");
        }
        else if(notifMessage.Contains("sensor 3")){
            sensor = GameObject.Find("sensor 3");
        }
        else if(notifMessage.Contains("sensor 4")){
            sensor = GameObject.Find("sensor 4");
        }
        else if(notifMessage.Contains("sensor 5")){
            sensor = GameObject.Find("sensor 5");
        }
        else{
            Debug.Log("something went wrong: " + notifMessage);
        }
    }

    public void NotificationClick(){
        StartCoroutine(customAnimations.NotificationEnd());
        PointerScript.target = sensor;
        pointerScript.Activate();
    }
}
