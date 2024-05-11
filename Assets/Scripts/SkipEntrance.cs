using UnityEngine;

public class SkipEntrance : MonoBehaviour
{
    // Reference to the Canvas component
    public Canvas canvas;

    // Function to disable the Canvas
    public void DisableCanvas()
    {
        if(canvas != null)
        {
            canvas.enabled = false;
        }
        else
        {
            Debug.LogError("Canvas reference not set!");
        }
    }
}

