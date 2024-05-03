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
	
using UnityEngine;

public class InGameInterface : MonoBehaviour
{

	// Variables to be set in Unity.
	[Tooltip("Player score script holding the amount of view and likes.")]
	[SerializeField] PlayerScore playerScore;

	[Tooltip("Likes counter text object.")]
	[SerializeField] TMPro.TextMeshProUGUI likesCounterText;

	[Tooltip("Likes counter text object.")]
	[SerializeField] TMPro.TextMeshProUGUI viewersCounterText;

	[Tooltip("Chat box wrapper object.")]
	[SerializeField] GameObject chatBoxWrapper;

	[Tooltip("Top bar object. Used to trigger recalculation of position (to resovle Unity bug).")]
	[SerializeField] GameObject topBar;

    [Tooltip("Message prefab.")]
    [SerializeField] GameObject chatMessagePrefab;

	[Tooltip("Sprite folder. Example: \"Assets/Resources/Images\"")]
	[SerializeField] string spriteFolder;

	[Tooltip("Audio clip to play when a message is printed.")]
	[SerializeField] AudioClip audioClip;

	[Tooltip("Message volume.")]
	[SerializeField] float audioVolume = 1.0f;

	[Tooltip("Username text object.")]
	[SerializeField] TMPro.TextMeshProUGUI usernameText;
	private AudioSource audioSource;

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
		if (chatBoxWrapper == null) {
			Debug.LogWarning("No chatBoxWrapper selected. Thus the chat will be disabled.");
			DestroyDueToError();
		}

		// Find the child GameObject with the name "Username".
		if (usernameText == null) {
			Debug.LogWarning("Username text object is not set. No username will be displayed.");
		}

		// Check if sprite folder is set.
		if (spriteFolder == null)
			Debug.LogWarning("Sprite folder is not set. No chat messages will be displayed.");
		else
			spriteFolder = spriteFolder.Replace("Assets/", "").Replace("Resources/", ""); // Remove "Assets/" and "Resources/" from the path.

		// Nested function for convenience.
		void DestroyDueToError()
		{
			Debug.LogError("InGameInterface: Due to missing elemenet, this script will be destroyed.");
			Destroy(this); // Destroy this script if the required component is not found.
		}

		// Set username.
		if (usernameText != null)
			usernameText.text = Storage.GetUsername();

		// Set audio source.
		if (audioClip != null) {
			if (audioSource == null) {
				audioSource = gameObject.AddComponent<AudioSource>();
				audioSource.clip = audioClip;
			}
			SetMessageVolume(audioVolume);
		}
	}

	// Update is called once per frame
	void Update()
	{
		UpdateInGameInterface(); // Call main function of file.
	}

	// Set message volume.
	void SetMessageVolume(float volume) {
		if (audioSource != null)
			audioSource.volume = volume;
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
		likesCounterText.text = Formatting.FloatToShortString(playerScore.likes);
		viewersCounterText.text = Formatting.FloatToShortString(playerScore.viewers);

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

		if (chatBoxWrapper == null)
			return;

		// Check arguments.
		if (message == null || sprite == null) {
			Debug.LogError("PrintMessage: One or more arguments are null.");
			return;
		}

		// Clean sprite name.
		sprite = sprite.Split('.')[0]; // Remove file extension.

		// Get the chat box object.
		GameObject chatBox = chatBoxWrapper.transform.Find("Chat Box").gameObject; // Get the chat box object.

		// Get sprite resources.
		string spritePath = spriteFolder + "/" + sprite;
		Sprite spriteResources = Resources.Load<Sprite>(spritePath);
		if (spriteResources == null) {
			Debug.LogError("PrintMessage: Sprite located at \"" + spritePath + "\" could not be found. Message won't be printed.");
			return;
		}

		// Create a new chat message object.
        GameObject messageObject = Instantiate(chatMessagePrefab, transform);
		messageObject.transform.SetParent(chatBox.transform);
		messageObject.transform.SetAsFirstSibling();
		
		TMPro.TextMeshProUGUI messageObjectTextComponent =
			messageObject.transform
			.Find("Text Wrapper").Find("Text")
			.gameObject.GetComponent<TMPro.TextMeshProUGUI>();

		UnityEngine.UI.Image messageObjectAvatarComponent =
			messageObject.transform
			.Find("Avatar")
			.gameObject.GetComponent<UnityEngine.UI.Image>();

		// Add content to new message.
		messageObjectTextComponent.text = message;
		messageObjectAvatarComponent.sprite = spriteResources;

		// Scroll to the top.
		chatBox.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

		// Play audio clip.
		if (audioClip != null)
			audioSource.Play();
	}

}
