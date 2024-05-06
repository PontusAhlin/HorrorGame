/**
    * This file is used for animating the chat messages and makes use of
    * Vectore2.Lerp to do so.
    *
    * Author(s): William Fridh
    */

using System.Collections;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{

    [Tooltip("The duration of the animation.")]
    [SerializeField] float animationDuration = 1;
    
    private RectTransform rectTransform;
    private float percentage = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Get the RectTransform component.
        rectTransform = GetComponent<RectTransform>();
        // Set the scale to 0.
        rectTransform.localScale = new Vector3(0, 0, 0);
        // Start the animation.
        StartCoroutine(ScaleOverTime());
    }

    // Update is called once per frame.
    void Update()
    {
        if (percentage >= 1)
        {
            Destroy(gameObject);
        }
        else
        {
            percentage += Time.deltaTime / animationDuration;
        }
    }

    // Update is called once per frame.
    IEnumerator ScaleOverTime()
    {
        // Moves the GameObject from it's current position to destination over time.
        rectTransform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), percentage);
        yield return null;
    }

}
