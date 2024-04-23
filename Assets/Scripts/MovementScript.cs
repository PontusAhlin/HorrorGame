/**
    * This script is attached to the player object and is responsible for moving the player in the direction of the camera.
    * The player moves in the direction of the camera's forward vector, rotated by the camera's y rotation.
    * This allows the player to move in the direction they are looking as well as walk in stairs.
    * 
    * Authors: William Fridh, Alin-Cristian Serban
    */
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    // Public variables set in Unity Editor
    private Transform camTransform;
    [Tooltip("Player movement speed.")]
    [SerializeField] float speed = 1f;
    private Gamepad gamepad;            // We're using INPUTSYSTEM, configured it to imitate gamepad buttons touchscreens
    [Tooltip("Upper step ray element. This element is responsible for sending out a ray to check if the next step is rechable.")]
    [SerializeField] GameObject stepRayUpper;
    [Tooltip("Lower step ray element. This element is responsible for sending out a ray to check for step collision.")]
    [SerializeField] GameObject stepRayLower;
    private CapsuleCollider capsuleCollider;
    [Tooltip("Height of the step.")]
    [SerializeField] float stepHeight = 0.6f;             // Height of the step
    [Tooltip("Speed of the step/climbing (higher means faster, but less smooth).")]
    [SerializeField] float stepSpeed = 0.1f;              // Speed of the step
    [Tooltip("Margin of the lower step ray (margin from ground).")]
    [SerializeField] float stepRayLowerMargin = 0.1f;     // Margin of the lower step ray (margin from ground)

    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;                              // Select current gamepad
        capsuleCollider = GetComponent<CapsuleCollider>();      // Get the player's capsule collider

        Camera childCam = GetComponentInChildren<Camera>();     // Find child camera
        if (childCam != null) {
            camTransform = childCam.transform;                  // Get the camera's transform
        } else {
            Debug.LogError("No child Camera found");
        }

        // Inform develoepr of faulty settings.
        if (stepRayLowerMargin == 0)
            Debug.LogWarning("stepRayLowerMargin is 0, this may cause the player to not be able to climb steps.");
    }

    // Update is called once per frame
    void Update()
    {

        // We're using INPUTSYSTEM, configured it to imitate gamepad buttons touchscreens
        if (gamepad == null)
            return; // No gamepad connected


        // Set the position of the stepRay objects to the player's position.
        // Important to keep them on the same level and at correct height.
        stepRayLower.transform.position = transform.position - new Vector3(0f, capsuleCollider.height / 2 - capsuleCollider.center.y, 0f) + new Vector3(0f, stepRayLowerMargin, 0f);
        stepRayUpper.transform.position = stepRayLower.transform.position + new Vector3(0f, stepHeight, 0f);

        // Rotate the stepRay objects to match the camera's rotation.
        // Required to get the raycasting to work.
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