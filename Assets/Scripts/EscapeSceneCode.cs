using System.Collections;
using System.Collections.Generic;
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
    }
    void Update(){
                scoreText.text = "SCORE: " + score.ToString();
    }
    public void BackToMenu(){
        Debug.Log("pls work");
        SceneManager.LoadScene("MainMenu");
    }
}
