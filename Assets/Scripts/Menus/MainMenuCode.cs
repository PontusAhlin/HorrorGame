/**
    * MainMenuCode.cs
    * Author(s): Arnob Sarker, William Fridh
    */

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject CreditsMenu;
    [SerializeField] string HighscoreSceneName;
    [Tooltip("The name of the scene to load when the player wants to see the highscore.")]
    [SerializeField] public string StartingSceneName = "Entrance";
    public InputField playername;

    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
    }

    public void PlayNowButton()
    {
        //Debug.Log("Player name is: " + playername.text);
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene(StartingSceneName);
    }

    public void CreditsButton()
    {
        // Show Credits Menu
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
        Debug.Log("Credits");
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
    }

    public void HighscoreButton()
    {
        // Show Highscore Menu
        if (HighscoreSceneName != null && HighscoreSceneName != "")
        {
            SceneManager.LoadScene(HighscoreSceneName);
        }
        else
        {
            Debug.LogError("HighscoreSceneName is not set in the inspector, thus no navigation will be done.");
        }
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
        Debug.Log("Ended");
    }

    // Update is called once per frame
    void Update()
    {
        // Handle input using new Input System
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            QuitButton();
        }
    }
}
