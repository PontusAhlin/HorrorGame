/**
    * Player Score.
    * 
    * This script is used to keep track of the player's viewers and likes.
    * It's values are adjusted upon by MonsterGenerateViews.cs that
    * increases and decreases the amount of viewers.
    *
    * Note how the viewers if of type for for a more percice calculation.
    *
    * Autor(s): William Fridh
    */

using UnityEngine;

public class PlayerScore : MonoBehaviour
{

    [Tooltip("Total amount of viewers the player has.")]
    public float viewers = 0;                                     // Amount of score generators (a.k.a. viewers).
    [Tooltip("Total amount of likes the player has.")]
    public int likes = 0;                                       // The players likes.
    [Tooltip("Amount fo likes per view that should be generated during each iteration.")]
    [SerializeField] float likesPerViewer = 1.0f;               // Multiplier for score generation.
    [Tooltip("The interval timer at which more likes should be generated.")]
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
