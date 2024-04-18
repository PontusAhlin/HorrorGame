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
    private Gamepad gamepad;            // We're using INPUTSYSTEM, configured it to imitate gamepad buttons touchscreens
    public GameObject stepRayUpper;
    public GameObject stepRayLower;
    public float stepHeight = 0.6f;     // Height of the step
    public float stepSpeed = 0.1f;
    public float stepRayLowerMargin = 0.1f;
    private CapsuleCollider capsuleCollider;

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


        capsuleCollider = GetComponent<CapsuleCollider>();

        //stepRayLower.transform.position = transform.position - new Vector3(0f, captulseColliderHeight / 2, 0f) + new Vector3(0f, stepRayLowerMargin, 0f);

        //stepRayUpper.transform.position = stepRayLower.transform.position + new Vector3(0f, stepHeight, 0f);

        if (stepRayLowerMargin == 0)
            Debug.LogWarning("stepRayLowerMargin is 0, this may cause the player to not be able to climb steps.");
    }

    // Update is called once per frame
    void Update()
    {

        stepRayLower.transform.position = transform.position - new Vector3(0f, capsuleCollider.height / 2 - capsuleCollider.center.y, 0f) + new Vector3(0f, stepRayLowerMargin, 0f);
        stepRayUpper.transform.position = stepRayLower.transform.position + new Vector3(0f, stepHeight, 0f);

        // We're using INPUTSYSTEM, configured it to imitate gamepad buttons touchscreens
        if (gamepad == null)
            return; // No gamepad connected


        stepRayUpper.transform.rotation = Quaternion.Euler(stepRayUpper.transform.rotation.eulerAngles.x, camTransform.rotation.eulerAngles.y, stepRayUpper.transform.rotation.eulerAngles.z);
        stepRayLower.transform.rotation = Quaternion.Euler(stepRayLower.transform.rotation.eulerAngles.x, camTransform.rotation.eulerAngles.y, stepRayLower.transform.rotation.eulerAngles.z);

        if (gamepad.buttonNorth.isPressed) {                                            // "butttonNorth" is our current movement button
            stepClimb();                                                                // Perform climb
            float targetAngle = camTransform.eulerAngles.y;                             // Get the camera's y rotation
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;  // Rotate the forward vector by the camera's y rotation
            transform.position += moveDir.normalized * speed * Time.deltaTime;          // Move the player in the direction of the rotated forward vector
        }
    }


    /**
        * Climb steps.
        *
        * This method is called when the player is trying to climb a step.
        * It checks if there is a step in front of the player and moves the player up if there is.
        *
        * Authors: William Fridh
        * Source: https://www.youtube.com/watch?v=DrFk5Q_IwG0
        */
    void stepClimb() {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, stepRayLower.transform.TransformDirection(Vector3.forward), out hitLower, capsuleCollider.radius + 0.1f)) {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, stepRayUpper.transform.TransformDirection(Vector3.forward), out hitUpper, capsuleCollider.radius + 0.3f)) {
                transform.position += new Vector3(0f, stepSpeed, 0f);
                Debug.LogWarning("JUMP");
            }
        }
    }
}