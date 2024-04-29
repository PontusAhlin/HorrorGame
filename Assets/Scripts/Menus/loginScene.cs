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

public class LoginScene : MonoBehaviour
{

    [Tooltip("The scene to load when the user logs in.")]
    [SerializeField] string gameScene;
    [Tooltip("The input field for the username.")]
    [SerializeField] TMPro.TMP_InputField usernameInput;
    [Tooltip("The scene to load upon cancel.")]
    [SerializeField] string cancelScene;
    [Tooltip("The error box object.")]
    [SerializeField] GameObject errorBox;

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
        usernameInput.text = Storage.GetUsername();

        // Disable error box.
        errorBox.SetActive(false);
    }

    // Go Live
    public void GoLive()
    {
        if (usernameInput.text == "")
        {
            errorBox.SetActive(true);
            TMPro.TextMeshProUGUI textComponent = errorBox.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            textComponent.text = "Please enter a username.";
            return;
        }
        if (usernameInput.text.Length < 3)
        {
            errorBox.SetActive(true);
            TMPro.TextMeshProUGUI textComponent = errorBox.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            textComponent.text = "Username is too short.";
            return;
        }
        SaveUsername();
        SceneManager.LoadScene(gameScene);
    }

    // Cancel
    public void Cancel()
    {
        SceneManager.LoadScene(cancelScene);
    }

    // Save username
    private void SaveUsername() {
        Storage.SetUsername(usernameInput.text);
    }
}
