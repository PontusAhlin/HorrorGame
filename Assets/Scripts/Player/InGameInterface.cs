/**
	* This file is used for chanding the content of the in game interface.
	* It's supposed to be called during each frame and adjust based on that.
	* It will for instance update the amount of viewers and likes.
	*
	* TODO:
	* - 
	*
	* Author(s): William Fridh
	*/

using UnityEditor.EditorTools;
using UnityEngine;

public class InGameInterface : MonoBehaviour
{

	// Variables to be set in Unity.
	[Tooltip("Player score script holding the amount of view and likes.")]
	[SerializeField] PlayerScore playerScore;
	[Tooltip("Likes counter text object. Automatically set to object name \"Likes Text\" if not set in Unity.")]
	[SerializeField] TMPro.TextMeshProUGUI likesCounterText;
	[Tooltip("Likes counter text object. Automatically set to object name \"Viewers Text\" if not set in Unity.")]
	[SerializeField] TMPro.TextMeshProUGUI viewersCounterText;
	[Tooltip("Chat box object. Automatically set to object name \"Chat Box\" if not set in Unity.")]
	[SerializeField] GameObject chatBox;
	[Tooltip("Top bar object. Used to trigger recalculation of position (to resovle Unity bug).")]
	[SerializeField] GameObject topBar;
	[Tooltip("Username text object where the given username will be displayed.")]
	[SerializeField] TMPro.TextMeshProUGUI usernameText;
	private bool chatBoxEnabled = false; // If the chat box is enabled or not. This depends on the chatBox object being set.

	// Start is called before the first frame update
	void Start()
	{

		// Check if the playerScore GameObject is set.
		if (playerScore == null)
		{
			Debug.LogError("Player score is not set.");
			DestroyDueToError();
		}

		// Find the child GameObject with the name "Likes Text".
		if (likesCounterText == null) {
			Debug.LogError("Likes text object is not set.");
			DestroyDueToError();
		}

		// Find the child GameObject with the name "Viewers Text".
		if (viewersCounterText == null) {
			Debug.LogError("Viewers text object is not set.");
			DestroyDueToError();
		}

		// Find the child GameObject with the name "Chat Box".
		if (chatBox == null) {
			Transform chatBoxTransform = transform.Find("Chat Box");
			if (chatBoxTransform != null)
			{
				// Get the GameObject component from the GameObject
				chatBox = chatBoxTransform.gameObject;
			}
			else
			{
				Debug.LogWarning("No child GameObject named 'Chat Box' found. Thus the chat will be disabled.");
			}
		}

		// Find the child GameObject with the name "Username".
		if (usernameText == null) {
			Debug.LogError("Username text object is not set.");
			DestroyDueToError();
		}

		// Nested function for convenience.
		void DestroyDueToError()
		{
			Debug.LogError("Due to missing elemenet, this script will be destroyed.");
			Destroy(this); // Destroy this script if the required component is not found.
		}

		// Set username.
		usernameText.text = Storage.GetUsername();
	}

	// Update is called once per frame
	void Update()
	{
		UpdateInGameInterface(); // Call main function of file.
	}

	/**
		* Update In Game Interface.
		*
		* This is the main function of this file. It's supposed to be called during each
		* update (new frame) and adjust based on that. Thus it should be called upon
		* from the Update() function.
		*
		* Note: We keep this logic seperate from Update() to keep the code clean and easy
		* to read/easy to maintain!
		*/
	void UpdateInGameInterface() {
		// Update the likes and views counter.
		if (playerScore.likes >= 1000) {
			decimal likesRounded = System.Math.Round((decimal)(playerScore.likes / 1000), 1);		// Round to 1 decimal place.
			likesCounterText.text = likesRounded.ToString() + "K";									// Round to 1 decimal place and add "K" for thousands.
		} else {
			likesCounterText.text = ((int)playerScore.likes).ToString();								// If less than 1000, just print the number.
		}
		if (playerScore.viewers >= 1000) {
			decimal viewersRounded = System.Math.Round((decimal)(playerScore.viewers / 1000), 1);	// Round down to nearest whole number.
			viewersCounterText.text = viewersRounded.ToString() + "K";								// Round down to nearest whole number and add "K" for thousands.
		} else {
			viewersCounterText.text = ((int)playerScore.viewers).ToString();							// If less than 1000, just print the number.
		}

		// Enable and disable top bar element to trigger recalculation of position (to resovle Unity bug).
		if (topBar != null) {
			topBar.SetActive(false);
			topBar.SetActive(true);
		}
	}

	/**
		* Print message.
		*
		* This function prints a message based on given arguments to the chat box.
		* It can be used for printing out normal chat messages, and/or when
		* a user has liked the stream.
		*/
	void PrintMessage(string message, string sprite) {
		if (!chatBoxEnabled) return; // If the chat box is not enabled, return.
		// Work in progress...
	}
}
