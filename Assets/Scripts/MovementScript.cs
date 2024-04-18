/**
    * This script is attached to the player object and is responsible for moving the player in the direction of the camera.
    * The player moves in the direction of the camera's forward vector, rotated by the camera's y rotation.
    * This allows the player to move in the direction they are looking.
    * 
    * Authors: William Fridh, Alin-Cristian Serban
    */
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    // Public variables set in Unity Editor
    private Transform camTransform;
    public float speed = 1f;
    private Gamepad gamepad; // We're using INPUTSYSTEM, configured it to imitate gamepad buttons touchscreens

    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;                              // Select current gamepad
        Camera childCam = GetComponentInChildren<Camera>();     // Find child camera
        if (childCam != null) {
            camTransform = childCam.transform;                  // Get the camera's transform
        } else {
            Debug.LogError("No child Camera found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //var gamepad = Gamepad.current; // We're using INPUTSYSTEM, configured it to imitate gamepad buttons touchscreens
        if (gamepad == null)
            return; // No gamepad connected

        if (gamepad.buttonNorth.isPressed) {                                            // "butttonNorth" is our current movement button
            float targetAngle = camTransform.eulerAngles.y;                             // Get the camera's y rotation
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;  // Rotate the forward vector by the camera's y rotation
            transform.position += moveDir.normalized * speed * Time.deltaTime;          // Move the player in the direction of the rotated forward vector
        }
    }
}