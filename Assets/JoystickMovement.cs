using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class JoystickMovement : MonoBehaviour
{

    //References to the joystick to get directions of joystick 
    Vector2 direction;
    float directionX;
    float directionY;
    private Transform camTransform;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private PlayerMovement playerMovement; 



    // Start is called before the first frame update
    void Start()
    {
        Transform camTransform = GetComponent<Transform>();
        Camera childCam = GetComponentInChildren<Camera>();

        if (childCam != null) {
            camTransform = childCam.transform;
        }
        capsuleCollider = GetComponent<CapsuleCollider>();      // Get the player's capsule collider
        FloatingJoystick joystick = GetComponent<FloatingJoystick>();
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }


    void MovePlayer(){
        
        direction = joystick.Direction;  
        print(direction);
        directionX = direction.x; 
        directionY = direction.y; 
        
        Vector3 camEuler = camTransform.rotation.eulerAngles;
        float targetAngle = camTransform.eulerAngles.y;                             // Get the camera's y rotation
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;  // Rotate the forward vector by the camera's y rotation
        transform.position += moveDir.normalized * playerMovement.speed * Time.deltaTime;


        //if(!(directionX > (Math.Sqrt(2)/2) && directionY > (Math.Sqrt(2)/2)) && (directionX > (-Math.Sqrt(2)/2) && directionY > (Math.Sqrt(2)/2))){
        //    playerMovement.speed = 1f;
        //}


    }


}

