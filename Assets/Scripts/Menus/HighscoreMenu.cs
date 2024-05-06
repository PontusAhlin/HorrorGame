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

        if (HighscoreList == null) {
            Debug.LogError("HighscoreList is not set in the inspector.");
            Destroy(this);
        }

        // Show the missing highscore text by default.
        MissingHighscoreTextHolder.SetActive(true);

        // Print the highscore.
        PrintHighscore();
    }

    void PrintHighscore()
    {
        // Get the highscore from the playerprefs
        // Remember to hide the missing highscore text if there's a highscore.
        string[] topFiveHighscores = Storage.GetTopFiveHighscore();
        foreach (string highscore in topFiveHighscores)
        {
            // Hide missing highscore text.
            MissingHighscoreTextHolder.SetActive(false);
            // Create a new highscore list object.
            GameObject highscoreListObject = Instantiate(HighscoreListObjectPrefab, transform);
            // Set the username.
            TMPro.TextMeshProUGUI usernameText = highscoreListObject.transform.Find("Username").GetComponent<TMPro.TextMeshProUGUI>();
            usernameText.text = highscore.Split(':')[0];
            // Set the score (rounded).
            TMPro.TextMeshProUGUI scoreText = highscoreListObject.transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
            int score = int.Parse(highscore.Split(':')[1]);
            scoreText.text = Formatting.FloatToShortString(score);
            // Set the parent.
            highscoreListObject.transform.SetParent(HighscoreList.transform);
        }
    }

    public void cancel() {
        SceneManager.LoadScene(QuiteMenuSceneName);
    }
}
