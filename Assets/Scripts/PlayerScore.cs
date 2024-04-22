/**
    * Player Score.
    * 
    * This script is used to keep track of the player's viewers and likes.
    * It's values are adjusted upon by MonsterGenerateViews.cs that
    * increases and decreases the amount of viewers.
    *
    * Autor(s): William Fridh
    */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{

    public int viewers = 0;                                     // Amount of score generators (a.k.a. viewers).
    public int likes = 0;                                       // The players likes.
    [SerializeField] float likesPerViewer = 1.0f;               // Multiplier for score generation.
    [SerializeField] float likeGenerationInterval = 1.0f;       // Multiplier for score generation.

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("IncreaseLikes", likeGenerationInterval, likeGenerationInterval);             // Increase score every second.
    }

    /**
     * Increase likes.
     *
     * This function holds the logic for increasing the likes. It's used intead of
     * Update() as this functionality should'nt be dependent on the frame rate.
     */
    private void IncreaseLikes()
    {
        // Linear increase of score generators.
        likes += (int)(viewers * likesPerViewer);
        // Logging for debugging.
        Debug.Log("Viewers: " + viewers + "Likes: " + likes);
    }
}
