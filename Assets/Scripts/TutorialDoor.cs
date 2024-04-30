using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Ball") || other.CompareTag("Player")){
            SceneManager.LoadScene("MainMapRandom");
        }
    }
}
