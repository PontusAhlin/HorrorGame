using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
this script...
IS WHERE: attached to LorePaper's trigger.
DOES:
1. instantiates a random image inside it's prefab to display, titled 90 degrees on the canvas so it's invisible
2. checks if the player is looking down at AngleToReadAt and if it is,
3. twists the image back 90 degrees to display it, and undisplays when that stops being true

author: Alin-Cristian Serban
*/

public class PaperScript : MonoBehaviour
{
    [Tooltip("this number describes the angle that accepts looking down at the paper. 0 means you need to look straight down, 90 means any angle from straight forward to down")]
    public int AngleToReadAt;

    static private GameObject playerObject;
    private Transform playerTransform;

    private GameObject loreNoteParent;

    private GameObject uiPaper;

    [Tooltip("this is a list of UI images to be displayed on the screen and picked from when a paper spawns")]
    public List<GameObject> paperNotes;

    void Start()
    {
        loreNoteParent = GameObject.Find("LoreNotes");
        GameObject playerObject = GameObject.Find("Main Camera");
        playerTransform = playerObject.transform; // Reference to the player's transform, needed for angle checking
        //spawn the paper in the canvas but twisted so its invis
        uiPaper = Instantiate(paperNotes[UnityEngine.Random.Range(0,paperNotes.Count)], Vector3.zero, Quaternion.Euler(0f,90f,0f)) as GameObject;
        //and parent it
        uiPaper.transform.SetParent(loreNoteParent.transform, false);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            { 
                //dumb math because unity's 0 angle is in front, looking down is ~0-85 degrees, and looking up is ~360-270 degrees
            if ((90 - playerTransform.localEulerAngles.x < AngleToReadAt) && ((90 - playerTransform.localEulerAngles.x) > 0))
            {
                uiPaper.transform.rotation = Quaternion.Euler(0,0,0); //display image
            }
            else
            {
                uiPaper.transform.rotation = Quaternion.Euler(0,90f,0); //hide image
            }
        }
    }
}