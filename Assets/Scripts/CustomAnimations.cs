/**
    * This file is used for animating animations of UI elements and makes use of
    * Vectore2.Lerp(), Coroutine, and Time.deltaTime.
    * Simply attach this script to a GameObject with a RectTransform component
    * to animate it.
    *
    * The animation can run thanks to coroutines that are used to animate the
    * GameObject. The animation can be of different types, such as scaling from
    * 0 to 1, or moving from one position to another.
    *
    * Time.deltaTime is used to make the animation smooth and independent of the
    * frame rate. It does not make use of the Update() function for this as it
    * is recommended to control animations with coroutines.
    *
    * Author(s): William Fridh
    */

using System.Collections;
using UnityEngine;

public class CustomAnimations : MonoBehaviour
{

    [Tooltip("The duration of the animation (s).")]
    [SerializeField] float animationDuration = 1;

    [Tooltip("The type of animation to perform.")]
    enum AnimationType { ScaleZeroToOne };
    [SerializeField] AnimationType animationType;
    
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        // Get the RectTransform component.
        rectTransform = GetComponent<RectTransform>();
        // Start the animation.
        switch (animationType)
        {
            case AnimationType.ScaleZeroToOne:
                StartCoroutine(ScaleZeroToOne());
                break;
        }
    }

    /**
        * This function is used to animate the scale of the GameObject
        * from 0 to 1. It does so in a linear fashion.
        */
    IEnumerator ScaleZeroToOne()
    {
        float currentTime = 0.0f;
        Vector3 startScale = new Vector3(0, 0, 0);
        Vector3 endScale = new Vector3(1, 1, 1);
        while (currentTime <= animationDuration)
        {
            rectTransform.localScale = Vector3.Lerp(startScale, endScale, currentTime/animationDuration);
            currentTime += Time.deltaTime;
            yield return null; // yield control back to Unity's main loop
        }
        rectTransform.localScale = endScale;
        yield return null;
    }

}
