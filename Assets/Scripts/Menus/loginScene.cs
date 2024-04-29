/**
    * loginScene.cs
    * This script is used to control the login scene and handle the
    * user input for the username.
    *
    * TODO:
    * - Add code to save the username to a database or file.
    * - Add code to load the username from a database or file.
    *
    * Author(s): William Fridh
    */

using UnityEngine;
using UnityEngine.SceneManagement;

public class loginScene : MonoBehaviour
{

    [Tooltip("The scene to load when the user logs in.")]
    [SerializeField] string gameScene;
    [Tooltip("The input field for the username.")]
    [SerializeField] TMPro.TMP_InputField usernameInput;
    [Tooltip("The scene to load upon cancel.")]
    [SerializeField] string cancelScene;

    // Start is called before the first frame update
    void Start()
    {
        if (gameScene == null)
        {
            Debug.LogError("Map Scene not set in the inspector.");
            Destroy(this);
        }
        if (usernameInput == null)
        {
            Debug.LogError("Username Input not set in the inspector.");
            Destroy(this);
        }
        if (cancelScene == null)
        {
            Debug.LogError("Cancel Scene not set in the inspector.");
            Destroy(this);
        }

        // Set value of input to last used (stored).
        //usernameInput.text = "Last Used Username";
    }

    // Go Live
    public void goLive()
    {
        SceneManager.LoadScene(gameScene);
    }

    // Cancel
    public void cancel()
    {
        saveUsername();
        SceneManager.LoadScene(cancelScene);
    }

    // Save username
    void saveUsername() {
        // Add code here to store the username in a database or file.
        // The data is found via usernameInput.text (I think).
    }
}
