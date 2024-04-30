/**
    * HighscoreMenu.cs
    * Script to handle the highscore menu.
    * 
    * Author(s): William Fridh
    */

using UnityEngine;

public class HighscoreMenu : MonoBehaviour
{

    [Tooltip("Textobject telling player there's no highscores.")]
    [SerializeField] GameObject MissingHighscoreTextHolder;

    [Tooltip("The prefab that will hold the highscore list.")]
    [SerializeField] GameObject HighscoreListObjectPrefab;

    [SerializeField] string QuiteMenuSceneName;

    // Start is called before the first frame update
    void Start()
    {

        if (MissingHighscoreTextHolder == null) {
            Debug.LogError("MissingHighscoreTextHolder is not set in the inspector.");
            Destroy(this);
        }

        if (HighscoreListObjectPrefab == null) {
            Debug.LogError("HighscoreListObjectPrefab is not set in the inspector.");
            Destroy(this);
        }

        if (QuiteMenuSceneName == null) {
            Debug.LogError("QuiteMenuSceneName is not set in the inspector.");
            Destroy(this);
        }

        // Show the missing highscore text by default.
        MissingHighscoreTextHolder.SetActive(true);

        printHighscore();
    }

    void printHighscore()
    {
        // Get the highscore from the playerprefs
        // Remember to hide the missing highscore text if there's a highscore.
    }

    public void quit() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(QuiteMenuSceneName);
    }
}
