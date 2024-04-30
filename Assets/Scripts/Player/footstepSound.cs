using UnityEngine;
using UnityEngine.InputSystem;

public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepsSound;
    private Gamepad gamepad;

    void Start()
    {
        // Get a reference to the Gamepad instance
        gamepad = Gamepad.current;
    }

    void Update()
    {
        // Check if the Gamepad instance exists and if the North button (A button on Xbox controller) is pressed
        if (gamepad != null && gamepad.buttonNorth.isPressed)
        {
            // Enable footsteps sound
            footstepsSound.enabled = true;
        }
        else
        {
            // Disable footsteps sound
            footstepsSound.enabled = false;
        }
    }
}
