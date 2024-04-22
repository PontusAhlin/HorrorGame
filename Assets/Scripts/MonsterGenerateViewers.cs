/**
 * This script is responsible for generating viewers based on the type of generation.
 * It can generate viewers linearly or exponentially. It also adds settings to monsters
 * such as:
 * - Amount of viewers generated per second.
 * - Speed of the viewer generation.
 * - Type of viewer generation.
 *
 * How to use:
 * - Attach this script to a monster object.
 * - Change inFieldOfView to true if the monster is in the field of view.
 * - Set the score object in the Unity Editor.
 *
 * Authors: William Fridh
 */

using UnityEngine;

public class MonsterGenerateViewers : MonoBehaviour
{

    [SerializeField] int viewersGenerated = 0;                          // Amount of viewers generated in total. [TODO: Make private]
    [SerializeField] PlayerScore PlayerScore;                           // Reference to the score script.
    [SerializeField] int viewersPerSecond = 1;                          // Amount of viewers generated per second.
    [SerializeField] float generatorSpeed = 1.0f;                       // Speed of the viewer generation.
    enum IncreaseAndDecrease { Linear, Exponential };                   // Type of viewer generation.
    [SerializeField] IncreaseAndDecrease increaseAndDecrease;
    public bool inFieldOfView;                                          // If the monster is in the field of view.

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerScore == null) {
            Debug.LogError("PlayerScore is not set. Removing MonsterGenerateViews script.");
            Destroy(this);
        }
        if (generatorSpeed == 0f)
            Debug.LogWarning("Generator speed is 0, which is invalid. It'll be set to 1 instead.");
            generatorSpeed = 1.0f;
        if (viewersPerSecond == 0) {
            Debug.LogWarning("viewersPerSecond speed is 0, which is invalid. It'll be set to 1 instead.");
            viewersPerSecond = 1;
        }
        // Add or remove viewers based on the current status.
        if (inFieldOfView)
            InvokeRepeating("AddViewers", generatorSpeed, generatorSpeed);
        else
            InvokeRepeating("RemoveViewers", generatorSpeed, generatorSpeed);
    }

    /**
        * Add viewers.
        *
        * This function add viewers based on the type of viewer generation.
        */
    void AddViewers() {
        Debug.Log("Adding viewers");
        int viewersToAdd = 0;                           // Create tmp. holder of viewvers to add.
        switch (increaseAndDecrease) {
            case IncreaseAndDecrease.Linear:
                viewersToAdd = viewersPerSecond;
                break;
            case IncreaseAndDecrease.Exponential:
                if (viewersGenerated == 0)                   
                    viewersToAdd = viewersPerSecond;    // If no viewers yet, add the viewersPerSecond.
                else
                    viewersToAdd *= viewersToAdd;       // Otherwise do it exponentially.
                break;
        }
        viewersGenerated += viewersToAdd;                // Save the amount of viewers generated.
        PlayerScore.viewers += viewersToAdd;             // Add the viewers to the score.
    }

    /**
        * Remove viewers.
        *
        * This function remove viewers based on the type of viewer generation.
        */
    void RemoveViewers() {
        Debug.Log("Removing viewers");

        if (viewersGenerated <= 0)
            return;                                         // If no viewers, return.

        int viewersToRemove = 0;                            // Create tmp. holder of viewvers to remove.

        switch (increaseAndDecrease) {
            case IncreaseAndDecrease.Linear:
                viewersToRemove = viewersPerSecond;
                break;
            case IncreaseAndDecrease.Exponential:
                if (viewersGenerated == 0)                   
                    viewersToRemove = viewersPerSecond;     // If no viewers yet, remove the viewersPerSecond.
                else
                    viewersToRemove *= viewersToRemove;     // Otherwise do it exponentially.
                break;
        }
        viewersGenerated -= viewersToRemove;                 // Save the amount of viewers generated.
        PlayerScore.viewers -= viewersToRemove;                   // Remove the viewers to the score.
    }

}

