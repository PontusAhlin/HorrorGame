/**
    * Tutorial Door.
    *
    * This script is used to handle the tutorial door.
    * When the player enters the door, the achievement progress
    * is increased and the next scene is loaded.
    *
    * Author(s): Arnob Dey Sarker, William Fridh
    */
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialDoor : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Ball") || other.CompareTag("Player")){
            // Change achievement progress.
            GameObject achievementHolder = other.gameObject.transform.parent.Find("AchievementHolder").gameObject;
            AchievementHandler(achievementHolder);
            // Load next scene.
            SceneManager.LoadScene("MainMapRandom");
        }
    }

    private void AchievementHandler(GameObject achievementHolder)
    {
        // Get component.
        AchievementAbstract AchievementFive = achievementHolder.transform.Find("AchievementFive").GetComponent<AchievementAbstract>();
        // Increase progress.
        AchievementFive.AddProgress(1);
    }
}
