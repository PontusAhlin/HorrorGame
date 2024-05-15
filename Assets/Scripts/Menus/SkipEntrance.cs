/**
    * A simple script to jump into the entrance scene from the tutorial 
    * Authors: Arnob Dey Sarker, Pontus Åhlin
*/

using UnityEngine;

public class SkipEntrance : MonoBehaviour
{
    // Reference to the Canvas component
    public Canvas canvas;
    private float skipTimer;
    // Function to disable the Canvas

    void Start(){
        skipTimer = 38.5f;
    }


    void FixedUpdate(){

        //Added a timer to make the scene "run out"
        if(skipTimer < 1f){
            DisableCanvas();
        }
        skipTimer -= Time.deltaTime;
    }

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

