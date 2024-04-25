using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameEndCode : MonoBehaviour
{
        public GameObject gameEnd;

    // Start is called before the first frame update
    void Start()
    {
        gameEnd = GameObject.Find("GameEndCanvas");
        
        gameEnd.GetComponent<Canvas>().enabled = true;
    }

    public void MainMenubutton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        gameEnd.GetComponent<Canvas>().enabled = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
