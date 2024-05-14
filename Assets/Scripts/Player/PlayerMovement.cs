/**
    * This script is attached to the player object and is responsible for moving the player in the direction of the camera.
    * The player moves in the direction of the camera's forward vector, rotated by the camera's y rotation.
    * This allows the player to move in the direction they are looking as well as walk in stairs.
    * The movement is done using a joystick structure, the joystick prefab can be found here: https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631#content
    *
    * Note that this file can be written without rotating the objects used for raycasting, but it's easier to understand this way.
    * 
    * Authors: William Fridh, Alin-Cristian Serban, Pontus Åhlin
    */
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMovement : MonoBehaviour
{
    // Public variables set in Unity Editor
    private Transform camTransform;
    [Tooltip("Player movement speed.")]

    [SerializeField] private float speed = 1f;
    public float speedInit = 1f;
    
    private Gamepad gamepad;            // InputSystem is used and set to imitate controller input on touchscreens.

    [Tooltip("Upper step ray element. This element is responsible for sending out a ray to check if the next step is rechable.")]
    [SerializeField] GameObject stepRayUpper;

    [Tooltip("Lower step ray element. This element is responsible for sending out a ray to check for step collision.")]
    [SerializeField] GameObject stepRayLower;
    
    [Tooltip("Height of the step.")]
    [SerializeField] float stepHeight = 0.6f;             // Height of the step

    [Tooltip("Speed of the step/climbing (higher means faster, but less smooth).")]
    [SerializeField] float stepSpeed = 0.1f;              // Speed of the step

    [Tooltip("Margin of the lower step ray (margin from ground).")]
    [SerializeField] float stepRayLowerMargin = 0.1f;     // Margin of the lower step ray (margin from ground)

    private CapsuleCollider capsuleCollider;
    private float lowerRayCastingDistance = 0.1f;        // Distance of the lower raycasting
    private float upperRayCastingDistance = 0.3f;        // Distance of the upper raycasting


    private Vector2 direction;          //Used for the direction of the joystick
    private float directionX;
    private float directionY;
    [Tooltip("Documentation for the used joystick pack https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631#content")]
    [SerializeField] private FloatingJoystick joystick;


    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;                              // Select current gamepad
        capsuleCollider = GetComponent<CapsuleCollider>();      // Get the player's capsule collider
        FloatingJoystick joystick = GetComponent<FloatingJoystick>();

        // Find child camera
        Camera childCam = GetComponentInChildren<Camera>();
        if (childCam != null) {
            camTransform = childCam.transform;
        } else {
            Debug.LogError("No child Camera found");
            DestroyDueToError();
        }

        if (speed == 0)
            Debug.LogWarning("Speed is 0, player will not move.");

        if (camTransform == null) {
            Debug.LogError("camTransform is null.");
            DestroyDueToError();
        }

        if (stepRayUpper == null || stepRayLower == null) {
            Debug.LogError("StepRay objects are null.");
            DestroyDueToError();
        }

        if (capsuleCollider == null) {
            Debug.LogError("CapsuleCollider is null.");
            DestroyDueToError();
        }

        if (stepHeight == 0) {
            Debug.LogWarning("StepHeight is 0, player will not be able to climb steps.");
        }

        if (stepRayLowerMargin == 0)
            Debug.LogWarning("stepRayLowerMargin is 0, this may cause the player to not be able to climb steps.");

        // Clear if required settings are missing.
        void DestroyDueToError() {
            Debug.LogError("PlayerMovement script is missing required settings thus it will be destroyed.");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

        /* Deprecated
        // Check if a gamepad is connected
        if (gamepad == null)
            return;
        */

        // Set the position of the stepRay objects to the player's position.
        // Important to keep them on the same level and at correct height.
        stepRayLower.transform.position = transform.position - new Vector3(0f, capsuleCollider.height/2 - capsuleCollider.center.y, 0f) + new Vector3(0f, stepRayLowerMargin, 0f);
        stepRayUpper.transform.position = stepRayLower.transform.position + new Vector3(0f, stepHeight, 0f);

        // Rotate the stepRay objects to match the camera's rotation.
        // Required to get the raycasting to work.
        Vector3 camEuler = camTransform.rotation.eulerAngles;
        stepRayUpper.transform.rotation = Quaternion.Euler(0f, camEuler.y, 0f);
        stepRayLower.transform.rotation = Quaternion.Euler(0f, camEuler.y, 0f);
        
        // Stiches together joystick/player movement direction
        MovePlayer();

        
        
        
        /* Deprecated
        if (gamepad.buttonNorth.isPressed) {                                            // "butttonNorth" is our current movement button
            StepClimb();                                                                // Perform climb
            float targetAngle = camTransform.eulerAngles.y;                             // Get the camera's y rotation
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;  // Rotate the forward vector by the camera's y rotation
            transform.position += moveDir.normalized * speed * Time.deltaTime;          // Move the player in the direction of the rotated forward vector
        }*/
    }

    /*
        * Checks different parts of the joystick and how  
        * much the player should move based on the joystick position
    */
    
    void MovePlayer(){
        
        StepClimb();

        // Gets the directions of the joystick
        direction = joystick.Direction;
        directionX = direction.x; 
        directionY = direction.y; 
        
        // These if statements checks different parts of the joystick
        // First one is top part of joystick
        if(directionY > (Math.Sqrt(3)/2) && (directionX > -1/2 || directionX < 1/2)){
            JoystickPlayerSpeed(1);
            return;
        }

        // Second one checks middle part of joystick/circle up until the first part 
        if(directionY > 1/2 && ((directionX > -Math.Sqrt(3)/2) || (directionX < Math.Sqrt(3)/2))){
            JoystickPlayerSpeed(1.3f);
            return;
        }

        // Third checks the under the middle line of the joystick/circle
        if(directionY < 0){
            JoystickPlayerSpeed(2);
            return;
        }

    }

    /*
        * Calculates the direction of the player while using the joystick, 
        * also divides the speed of the players original movespeed 
    */
    void JoystickPlayerSpeed(float speedDivision){

        // References to the camera and capsule collider 
        GameObject camera = GameObject.Find("Main Camera");
        Transform cameraTransform = camera.GetComponent<Transform>();    
        
        CapsuleCollider collider = GetComponent<CapsuleCollider>(); 
        Transform colliderTra = collider.GetComponent<Transform>();

        // Sets the speed speed based on speedDivision
        speed = speedInit/speedDivision;
        // Calculations to get the direction of how the player should move based on the joystick direction
        Vector3 moveDir = directionX * cameraTransform.right.normalized + directionY * cameraTransform.forward.normalized;
        colliderTra.position += moveDir * speed * Time.deltaTime;
        cameraTransform.transform.position = colliderTra.position + new Vector3(0,capsuleCollider.height - 2,0);
    }




    /**
        * Climb steps.
        *
        * This method is called when the player is trying to climb a step.
        * It checks if there is a step in front of the player and moves the player up if there is.
        *
        * Source: https://www.youtube.com/watch?v=DrFk5Q_IwG0
        */
    void StepClimb() {
        float radius = capsuleCollider.radius;
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, stepRayLower.transform.TransformDirection(Vector3.forward), out hitLower, radius + lowerRayCastingDistance)) {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, stepRayUpper.transform.TransformDirection(Vector3.forward), out hitUpper, radius + upperRayCastingDistance)) {
                transform.position += new Vector3(0f, stepSpeed, 0f);
            }
        }
    }
}

