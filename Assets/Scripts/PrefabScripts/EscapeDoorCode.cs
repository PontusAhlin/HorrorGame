using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeDoorCode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        //Debug.Log("Escape door war triggered");
        if(other.CompareTag("Player")){
            Debug.Log("you escaped");
            SceneManager.LoadScene("EscapeScene");
        }
    }
}
