using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject CreditsMenu;
    [SerializeField] public string StartingSceneName = "MainMapRandom";
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
