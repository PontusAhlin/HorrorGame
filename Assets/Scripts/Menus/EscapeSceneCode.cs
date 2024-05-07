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

    [SerializeField] AchievementAbstract AchievementZero;
    [SerializeField] AchievementAbstract AchievementOne;
    [SerializeField] AchievementAbstract AchievementTwo;
    [SerializeField] AchievementAbstract AchievementThree;

    float score = 0;
    void Start(){
        score = ScoreManager.Score;
        scoreText.text = "SCORE: " + score.ToString();
        if (Storage.AddToHighscore(Storage.GetUsername() + ":" + score.ToString())) {
            // New highscore.
        }
        AchievementHandler();
    }

    //void Update(){
    //    scoreText.text = "SCORE: " + score.ToString();
    //}
    
    public void BackToMenu(){
    //    Debug.Log("pls work");
        SceneManager.LoadScene("MainMenu");
    }

    /**
     * This method is used to handle the achievements connected to
     * this section of the game.
     */
    private void AchievementHandler()
    {
        if (AchievementZero != null)
            AchievementZero.AddProgress(1);
        if (AchievementOne != null && score < 1)
            AchievementOne.AddProgress(1);
        if (AchievementTwo != null && Storage.GetLastGameViewers() >= 100000)
            AchievementTwo.AddProgress(1);
        if (AchievementThree != null && score >= 100000)
            AchievementThree.AddProgress(1);
    }
}
