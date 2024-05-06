using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
this script...
IS WHERE: attached to LorePaper's spherical trigger
DOES: checks if the player is looking at a far enough angle down to read the paper

author: Alin-Cristian Serban
*/

public class PaperScript : MonoBehaviour
{
    [Tooltip("this number describes the angle that accepts looking down at the paper. 0 means you need to look straight down, 90 means any angle from straight forward to down")]
    public int AngleToReadAt;

    static private GameObject playerObject;
    public Transform playerTransform;

    void Start()
    {
        GameObject playerObject = GameObject.Find("Main Camera");
        playerTransform = playerObject.transform; // Reference to the player's transform
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            { 
            if ((90 - playerTransform.localEulerAngles.x < AngleToReadAt) && ((90 - playerTransform.localEulerAngles.x) > 0))
            {
                Debug.Log(playerTransform.localEulerAngles.x);
            }
        }
    }
}
