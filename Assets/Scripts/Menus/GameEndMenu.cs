using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndMenu : MonoBehaviour
{
    // Function to handle the "Return to Main Menu" button click
    public void ReturnToMainMenu()
    {
        // Load the main menu scene (assuming the scene name is "MainMenu")
        SceneManager.LoadScene("MainMenu");
    }

    // Function to handle the "Quit Game" button click
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}