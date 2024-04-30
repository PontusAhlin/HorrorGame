/**
    * EscapeSceneCode.cs
    * This script is used to control the escape scene. It is responsible for
    * displaying the score of the player and allowing the player to return
    * to the main menu.
    * 
    * Author(s): Sai Chintapalli, William Fridh
    */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeSceneCode : MonoBehaviour
{   
    [SerializeField]
    Text scoreText;

    float score = 0;
    void Start(){
        score = ScoreManager.Score;
        scoreText.text = "SCORE: " + score.ToString();
        if (Storage.AddToTopFiveHighscore(Storage.GetUsername() + ":" + score.ToString())) {
            // New highscore.
        }
    }

    //void Update(){
    //    scoreText.text = "SCORE: " + score.ToString();
    //}
    
    public void BackToMenu(){
    //    Debug.Log("pls work");
        SceneManager.LoadScene("MainMenu");
    }
}
