using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeSceneCode : MonoBehaviour
{    
    public void BackToMenu(){
        Debug.Log("pls work");
        SceneManager.LoadScene("MainMenu");
    }
}
