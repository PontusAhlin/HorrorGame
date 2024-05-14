/**
    * HighscoreMenu.cs
    * Script to handle the highscore menu.
    * 
    * Author(s): William Fridh
    */

using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoreMenu : MonoBehaviour
{

    [Tooltip("Textobject telling player there's no highscores.")]
    [SerializeField] GameObject MissingHighscoreTextHolder;

    [Tooltip("The object that will hold the highscore list.")]
    [SerializeField] GameObject HighscoreList;

    [Tooltip("The prefab that will hold the highscore list.")]
    [SerializeField] GameObject HighscoreListObjectPrefab;
    
    [Tooltip("The name of the scene to load when the player wants to quit.")]
    [SerializeField] string QuiteMenuSceneName;

    private Storage storage;

    // Start is called before the first frame update
    void Start()
    {
        if (MissingHighscoreTextHolder == null) {
            Debug.LogError("MissingHighscoreTextHolder is not set in the inspector.");
            Destroy(this);
            return;
        }

        if (HighscoreListObjectPrefab == null) {
            Debug.LogError("HighscoreListObjectPrefab is not set in the inspector.");
            Destroy(this);
            return;
        }

        if (QuiteMenuSceneName == null) {
            Debug.LogError("QuiteMenuSceneName is not set in the inspector.");
            Destroy(this);
            return;
        }

        if (HighscoreList == null) {
            Debug.LogError("HighscoreList is not set in the inspector.");
            Destroy(this);
            return;
        }

        // Get storage object.
        storage = Storage.GetStorage();
        // Show the missing highscore text by default.
        MissingHighscoreTextHolder.SetActive(true);
        // Print the highscore.
        PrintHighscore();
    }

    void PrintHighscore()
    {
        // Get the highscore from the playerprefs
        // Remember to hide the missing highscore text if there's a highscore.
        string[] Highscores = storage.GetHighscore();
        int i = 0;
        foreach (string highscore in Highscores)
        {
            // Print max 5 highscores.
            if (i++ == 5)
                break;
            // Hide missing highscore text.
            MissingHighscoreTextHolder.SetActive(false);
            // Create a new highscore list object.
            GameObject highscoreListObject = Instantiate(HighscoreListObjectPrefab, HighscoreList.transform);
            // Set the username.
            TMPro.TextMeshProUGUI usernameText = highscoreListObject.transform.Find("Username").GetComponent<TMPro.TextMeshProUGUI>();
            usernameText.text = highscore.Split(':')[0];
            // Set the score (rounded).
            TMPro.TextMeshProUGUI scoreText = highscoreListObject.transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
            int score = int.Parse(highscore.Split(':')[1]);
            scoreText.text = Formatting.FloatToShortString(score, 2);
        }
    }

    public void cancel() {
        SceneManager.LoadScene(QuiteMenuSceneName);
    }
}
