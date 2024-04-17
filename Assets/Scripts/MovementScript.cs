/**
    * This script is attached to the player object and is responsible for moving the player in the direction of the camera.
    * The player moves in the direction of the camera's forward vector, rotated by the camera's y rotation.
    * This allows the player to move in the direction they are looking.
    */
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    // Variables set in Unity Editor
    public Transform cam;
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        var gamepad = Gamepad.current; //we are using INPUTSYSTEM, and i've configured it to imitate gamepad buttons for our touchscreen
        if (gamepad == null)
            return; // No gamepad connected.

        if (gamepad.buttonNorth.isPressed) //north is our current movement button. so, this means "if this is pressed".
        {
        float targetAngle = cam.eulerAngles.y;                                      // Get the camera's y rotation
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;  // Rotate the forward vector by the camera's y rotation
        transform.position += moveDir.normalized * speed * Time.deltaTime;          // Move the player in the direction of the rotated forward vector
        }
    }
}