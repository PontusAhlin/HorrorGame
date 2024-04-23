using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeSceneCode : MonoBehaviour
{   
    [SerializeField]
    Text scoreText;

    [SerializeField]
    int score = 1337;
    void Start(){
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
