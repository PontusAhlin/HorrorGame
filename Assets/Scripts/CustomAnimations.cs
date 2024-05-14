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
    * Author(s): William Fridh, Sai Chintapalli
    */

using System.Collections;
using UnityEngine;

public class CustomAnimations : MonoBehaviour
{

    [Tooltip("The duration of the animation (s).")]
    [SerializeField] float animationDuration = 1;

    [Tooltip("The duration of the notification")]
    [SerializeField] float notificationDuration = 5;

    [Tooltip("The type of animation to perform.")]
    enum AnimationType { ScaleZeroToOne, Notification };
    [SerializeField] AnimationType animationType;
    
    private RectTransform rectTransform;

    private Vector3 StartPosition;

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
            case AnimationType.Notification:
                StartCoroutine(NotificationStart());
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

    public IEnumerator NotificationStart()
    {
        float currentTime = 0.0f;
        StartPosition = this.transform.position;
        Vector3 EndPosition = StartPosition - new Vector3(0,400,0);
        while (this.transform.position.y >= EndPosition.y)
         {
            this.transform.position -= new Vector3(0,3,0);
            currentTime += Time.deltaTime;
            yield return null; // yield control back to Unity's main loop
        }
        yield return null;
    }

    public IEnumerator NotificationEnd()
    {
        float currentTime = 0.0f;
        while (this.transform.position.y <= StartPosition.y + 400)
        {
            this.transform.position += new Vector3(0,4,0);
            currentTime += Time.deltaTime;
            yield return null; // yield control back to Unity's main loop
        }
        Destroy(gameObject);
        yield return null;
    }

    // IEnumerator Notification(){
    //     float currentTime = 0.0f;
    //     Vector3 StartPosition = this.transform.position;
    //     Vector3 EndPosition = StartPosition - new Vector3(0,70,0);
    //     while (this.transform.position.y >= EndPosition.y)
    //     {
    //         this.transform.position -= new Vector3(0,3,0);
    //         currentTime += Time.deltaTime;
    //         yield return null; // yield control back to Unity's main loop
    //     }
    //     //this.transform.position = EndPosition;
    //     yield return new WaitForSeconds(notificationDuration);
        
    //     currentTime = 0.0f;
    //     while (this.transform.position.y <= StartPosition.y + 200)
    //     {
    //         this.transform.position += new Vector3(0,4,0);
    //         currentTime += Time.deltaTime;
    //         yield return null; // yield control back to Unity's main loop
    //     }
    //     Destroy(gameObject);
    //     yield return null;
    // }

}
