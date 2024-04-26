using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    private bool gamePaused = false;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure GameManager persists between scenes
        }
        else
        {
            Destroy(gameObject); // If an instance already exists, destroy this one
        }
    }

    public void TogglePause()
    {
        gamePaused = !gamePaused;

        if (gamePaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        // You can also show pause menu UI if you have one
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        // Hide pause menu UI if it's visible
    }
}
