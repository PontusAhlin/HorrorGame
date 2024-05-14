using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectAppearDelay : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public float delayInSeconds = 2f;

    void Start()
    {
        // Start the coroutine to make the objects appear after a delay
        StartCoroutine(ShowObjectsAfterDelay());
        StartCoroutine(DisableTutorial());
    }

    IEnumerator ShowObjectsAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayInSeconds);

        // Enable both objects to make them appear
        object1.SetActive(true);
        object2.SetActive(true);
    }

    IEnumerator DisableTutorial()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayInSeconds);

        // Enable both objects to make them appear
        object3.SetActive(false);
    }


}