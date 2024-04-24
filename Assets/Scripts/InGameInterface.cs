/**
	* This file is used for chanding the content of the in game interface.
	* It's supposed to be called during each frame and adjust based on that.
	* It will for instance update the amount of viewers and likes.
	*
	* TODO:
	* - Add tooltips.
	*
	* Author(s): William Fridh
	*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameInterface : MonoBehaviour
{

	// Variables to be set in Unity.
	[Tooltip("Player score script holding the amount of view and likes.")]
	[SerializeField] PlayerScore playerScore;
	[SerializeField] TMPro.TextMeshPro likesCounterText;
	[SerializeField] TMPro.TextMeshPro viewersCounter;

	// Start is called before the first frame update
	void Start()
	{

		// Check if the playerScore GameObject is set.
		if (playerScore == null)
		{
			Debug.LogError("Player score GameObject is not set.");
			DestroyDueToError();
		}

		// Find the child GameObject with the name "Likes Text".
		Transform likesTextTransform = transform.Find("Likes Text");
		if (likesTextTransform != null)
		{
			// Get the TextMeshPro component from the GameObject
			likesCounterText = likesTextTransform.GetComponent<TMPro.TextMeshPro>();
		}
		else
		{
			Debug.LogError("No child GameObject named 'Likes Text' found.");
			DestroyDueToError();
		}

		// Find the child GameObject with the name "Viewers Text".
		Transform viewersTextTransform = transform.Find("Viewers Text");
		if (viewersTextTransform != null)
		{
			// Get the TextMeshPro component from the GameObject
			viewersCounter = viewersTextTransform.GetComponent<TMPro.TextMeshPro>();
		}
		else
		{
			Debug.LogError("No child GameObject named 'Viewers Text' found.");
			DestroyDueToError();
		}

		// Nested function for convenience.
		void DestroyDueToError()
		{
			Debug.LogError("Due to missing elemenet, this script will be destroyed.");
			Destroy(this); // Destroy this script if the required component is not found.
		}
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
		likesCounterText.text = playerScore.GetComponent<GameObject>().likes.ToString();
	}
}
