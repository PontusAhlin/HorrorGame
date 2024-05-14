/**
    * MainMenuCode.cs
    * Author(s): Arnob Sarker, William Fridh, Pontus Åhlin
    */

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject CreditsMenu;
    public GameObject TrademarksMenu;


    [Tooltip("The name of the scene to load when the player wants to see the highscore.")]
    [SerializeField] string HighscoreSceneName;

    [Tooltip("The name of the scene to load when the player wants to see the achievements.")]
    [SerializeField] public string AchievementsSceneName = "";

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
        TrademarksMenu.SetActive(false);

    }


    public void TrademarksButton()
    {
        // Displays the credits/trademarks screen 
        CreditsMenu.SetActive(false);
        MainMenu.SetActive(false);
        TrademarksMenu.SetActive(true);
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

    public void AchievementsButton()
    {
        // Show Achievements Menu
        if (AchievementsSceneName != null && AchievementsSceneName != "")
        {
            SceneManager.LoadScene(AchievementsSceneName);
        }
        else
        {
            Debug.LogError("AchievementsSceneName is not set in the inspector, thus no navigation will be done.");
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
