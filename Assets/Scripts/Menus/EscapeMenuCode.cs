/**
    * EscapeSceneCode.cs
    * This script is used to control the escape scene. It is responsible for
    * displaying the score of the player and allowing the player to return
    * to the main menu.
    * 
    * Author(s): Sai Chintapalli, William Fridh
    */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeSceneCode : MonoBehaviour
{   

    [Tooltip("Score text element.")]
    [SerializeField]
    Text scoreText;

    [Tooltip("The name of the scene to load when the player wants to quit.")]
    [SerializeField] string QuiteMenuSceneName;

    [SerializeField] AchievementAbstract AchievementZero;
    [SerializeField] AchievementAbstract AchievementOne;
    [SerializeField] AchievementAbstract AchievementTwo;
    [SerializeField] AchievementAbstract AchievementThree;

    // Private score data.
    private float likes;
    private float viewers;
    private Storage storage;

    // Start is called before the first frame update
    void Start(){
        // Get storage object.
        storage = Storage.GetStorage();
        // Get last game data.
        likes = storage.GetLastGameLikes();
        viewers = storage.GetLastGameViewers();
        // Display score (likes).
        scoreText.text = "SCORE: " + Formatting.FloatToShortString(likes, 3);
        if (likes > 0) {
            if (storage.AddToHighscore(storage.GetUsername() + ":" + likes.ToString())) {
                // New highscore.
            }
        }
        AchievementHandler();
    }
    
    /**
     * This method is used to return to the main menu.
     */
    public void BackToMenu(){
        SceneManager.LoadScene(QuiteMenuSceneName);
    }

    /**
     * This method is used to handle the achievements connected to
     * this section of the game.
     */
    private void AchievementHandler()
    {
        if (AchievementZero != null)
            AchievementZero.AddProgress(1);
        if (AchievementOne != null && likes < 1)
            AchievementOne.AddProgress(1);
        if (AchievementTwo != null && viewers >= 100000)
            AchievementTwo.AddProgress(1);
        if (AchievementThree != null && likes >= 100000)
            AchievementThree.AddProgress(1);
    }
}
