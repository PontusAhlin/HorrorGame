/**
    * EscapeDoorCode.cs
    * 
    * This script is attached to the escape door prefab.
    * It is used to detect when the player has reached the escape door
    * and then loads the escape menu scene as well as stores the player's
    * viewers and likes to a temporary location within storage.
    *
    * TODO:
    * - Add serialized fields instead. Hardcoded strings are bad!
    *
    * Author(s): Sai Chintapalli, William Fridh
    */

using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeDoorCode : MonoBehaviour
{

    string escapeMenuScene = "EscapeMenu";
    string pathToScoreScriptHolder = "XR Origin (AR Rig)/Camera Offset/Main Camera";

    private void OnTriggerEnter(Collider other) {
        //Debug.Log("Escape door war triggered");
        if (other.CompareTag("Player")) {
            //Debug.Log("you escaped");

            // Save the player's viewers and likes to storage
            if (pathToScoreScriptHolder == "")
                Debug.LogError("No path to score script holder set in the escape door prefab.");
            else {
                GameObject scoreScriptHolder = GameObject.Find(pathToScoreScriptHolder);
                scoreScriptHolder.GetComponent<PlayerScore>().StoreLikesAndViewers();
            }

            // Load the escape menu scene.
            if (escapeMenuScene != "")
                SceneManager.LoadScene(escapeMenuScene);
            else
                Debug.LogError("No escape menu scene set in the escape door prefab.");
        }
    }
}
