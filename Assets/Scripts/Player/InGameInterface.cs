/**
	* This file is used for chanding the content of the in game interface.
	* It's supposed to be called during each frame and adjust based on that.
	* It will for instance update the amount of viewers and likes.
	*
	* Author(s): William Fridh, Sai Chintapalli
	*/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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
	
	[Tooltip("Notification box wrapper object.")]
	[SerializeField] GameObject notificationBoxWrapper;

	[Tooltip("Top bar object. Used to trigger recalculation of position (to resovle Unity bug).")]
	[SerializeField] GameObject topBar;

    [Tooltip("Message prefab.")]
    [SerializeField] GameObject chatMessagePrefab;

	[Tooltip("Notification prefab.")]
    [SerializeField] GameObject notificationPrefab;

	[Tooltip("Sprite folder. Example: \"Assets/Resources/Images\"")]
	[SerializeField] string spriteFolder;

	[Tooltip("Audio clip to play when a message is printed.")]
	[SerializeField] AudioClip audioClip;

	[Tooltip("Message volume.")]
	[Range(0f, 1f)]
	[SerializeField] float audioVolume = 1.0f;

	[Tooltip("Username text object.")]
	[SerializeField] TMPro.TextMeshProUGUI usernameText;

	[Tooltip("Generate debugging messages.")]
	[SerializeField] bool generateDebugMessages = false;

	[Tooltip("Generate debugging messages speed.")]
	[SerializeField] float generateDebugMessagesSpeed = 5.0f;

	[Tooltip("The decimal for the viewers and the likes")]
	[SerializeField] int LikesAndViewerDecimal = 3;

	[Tooltip("Default color of the chat box.")]
	[SerializeField] Color defaultColor = new Color(0, 0, 0, 0.76f);

	private AudioSource audioSource;
	private Storage storage;
	private Sprite[] avatarSprites;
	private bool pinnedExists = false;

	// Start is called before the first frame update
	void Start()
	{
		// Get storage object.
		storage = Storage.GetStorage();

		// Check if the playerScore GameObject is set.
		if (playerScore == null)
			Debug.LogError("Player score is not set.");

		// Find the child GameObject with the name "Likes Text".
		if (likesCounterText == null)
			Debug.LogError("Likes text object is not set.");

		// Find the child GameObject with the name "Viewers Text".
		if (viewersCounterText == null)
			Debug.LogError("Viewers text object is not set.");

		// Find the child GameObject with the name "Chat Box".
		if (chatBoxWrapper == null)
			Debug.LogWarning("No chatBoxWrapper selected. Thus the chat will be disabled.");

		// Find the child GameObject with the name "Username".
		if (usernameText == null)
			Debug.LogWarning("Username text object is not set. No username will be displayed.");

		// Check if sprite folder is set.
		if (spriteFolder == null)
			Debug.LogWarning("Sprite folder is not set. No chat messages will be displayed.");
		else
			spriteFolder = spriteFolder.Replace("Assets/", "").Replace("Resources/", ""); // Remove "Assets/" and "Resources/" from the path.
	
		// Set username.
		if (usernameText != null)
			usernameText.text = storage.GetUsername();

		// Set audio source.
		if (audioClip != null) {
			if (audioSource == null) {
				audioSource = gameObject.AddComponent<AudioSource>();
				audioSource.clip = audioClip;
			}
			SetMessageVolume(audioVolume);
		}

		// Debugging.
		StartCoroutine(DebugPrintMessageCoroutine());

		// Load sprites.
		StartCoroutine(LoadSprites());
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

	IEnumerator LoadSprites()
	{
		// Load all sprites from the sprite folder.
		avatarSprites = Resources.LoadAll<Sprite>(spriteFolder + "/Avatars");
		if (avatarSprites == null)
			Debug.LogError("InGameInterface: No sprites found in the sprite folder.");
		yield return null;
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

		if (likesCounterText == null || viewersCounterText == null) {
			Debug.LogError("Missing element to display topbar correctly. Please check the inspector.");
			return;
		}

		// Update the likes and views counter.
		if (playerScore == null) { // Fall back on stored values if playerScore is not set.
			likesCounterText.text = Formatting.FloatToShortString(storage.GetLastGameLikes(),LikesAndViewerDecimal);
			viewersCounterText.text = Formatting.FloatToShortString(storage.GetLastGameViewers(),LikesAndViewerDecimal);

		} else {
			likesCounterText.text = Formatting.FloatToShortString(playerScore.likes,LikesAndViewerDecimal);
			viewersCounterText.text = Formatting.FloatToShortString(playerScore.viewers,LikesAndViewerDecimal);
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
	public void PrintMessage(string message, bool pin = false) {
		PrintMessage(message, pin, defaultColor);
	}
	public void PrintMessage(string message, bool pin, Color color) {

		if (chatBoxWrapper == null)
			return;

		// Check arguments.
		if (message == null) {
			Debug.LogError("PrintMessage: One or more arguments are null.");
			return;
		}

		// Get the chat box object.
		GameObject chatBox = chatBoxWrapper.transform.Find("Chat Box").gameObject; // Get the chat box object.

		// Create a new chat message object.
        GameObject messageObject = Instantiate(chatMessagePrefab, chatBox.transform);
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
		messageObjectAvatarComponent.sprite = avatarSprites[Random.Range(0, avatarSprites.Length)];

		// Set color.
		messageObject.gameObject.GetComponent<UnityEngine.UI.Image>().color = color;

		// Iterate each chat message.
		for (int i = 0; i < chatBox.transform.childCount; i++)
		{
			Transform child = chatBox.transform.GetChild(i);
			int maxI = 4;
			if (pinnedExists)
				maxI = 3;
			if (i == maxI) {
				if (child.gameObject.CompareTag("PinnedMessage")) {
					pinnedExists = true;
					// Remove old pinned messages.
					GameObject chatBoxWrapperLastChild = chatBoxWrapper.transform.GetChild(chatBoxWrapper.transform.childCount - 1).gameObject;
					if (chatBoxWrapperLastChild.CompareTag("PinnedMessage"))
						Destroy(chatBoxWrapperLastChild);
					// Add new pinned message to the bottom.
					child.transform.SetParent(chatBoxWrapper.transform);
					RectTransform messageObjectTransform = child.GetComponent<RectTransform>();
					messageObjectTransform.pivot = new Vector2(0, 0);
					messageObjectTransform.anchorMin = new Vector2(0, 0);
					messageObjectTransform.anchorMax = new Vector2(0, 0);
					messageObjectTransform.anchoredPosition = new Vector3(0, 0, 0);
				} else {
					Destroy(child.gameObject);
				}
			}
		}

		// Pin to bottom.
		if (pin) {
			// Add tag.
			messageObject.tag = "PinnedMessage";
		}

		// Scroll to the top.
		chatBox.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

		// Play audio clip.
		if (audioClip != null)
			audioSource.Play();
	}

	/**
		* Send notification.
		*
		* This function sends a notification to the notification box.
		* It can be used for sending out notifications to the user.
		*/
	public void SendNotification(string message, string sprite) {

		if (notificationBoxWrapper == null)
			return;

		// Check arguments.
		if (message == null) {
			Debug.LogError("PrintMessage: One or more arguments are null.");
			return;
		}

		// Clean sprite name.
		sprite = sprite.Split('.')[0]; // Remove file extension.

		// Get sprite resources.
		string spritePath = spriteFolder + "/" + sprite;
		Sprite spriteResources = Resources.Load<Sprite>(spritePath);
		if (spriteResources == null) {
			Debug.LogError("PrintMessage: Sprite located at \"" + spritePath + "\" could not be found. Message won't be printed.");
			return;
		}

		// Create a new notification object.
        GameObject messageObject = Instantiate(notificationPrefab, notificationBoxWrapper.transform);
		messageObject.transform.position = messageObject.transform.position + new Vector3(0, 80, 0);
		TMPro.TextMeshProUGUI messageObjectTextComponent =
			messageObject.transform
			.Find("Text Wrapper").Find("Text")
			.gameObject.GetComponent<TMPro.TextMeshProUGUI>();

		UnityEngine.UI.Image messageObjectAvatarComponent =
			messageObject.transform
			.Find("Avatar")
			.gameObject.GetComponent<UnityEngine.UI.Image>();

		// Add content to new notification.
		messageObjectTextComponent.text = message;
		messageObjectAvatarComponent.sprite = spriteResources;

		// Play audio clip.
		if (audioClip != null)
			audioSource.Play();
	}

	/**
		* Debugging function.
		*
		* Used to generate a random message for debugging purposes.
		*/
	IEnumerator DebugPrintMessageCoroutine()
	{
		while (true)
		{
			if (generateDebugMessages)
				PrintMessage("This is a test message.");
			yield return new WaitForSeconds(generateDebugMessagesSpeed);
		}
	}

}
