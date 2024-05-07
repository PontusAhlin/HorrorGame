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
        FloatingJoystick joystick = GetComponent<FloatingJoystick>();
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }


    void MovePlayer(){
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        
        GameObject camera = GameObject.Find("Main Camera");
        Transform cameraTransform = camera.GetComponent<Transform>();    
        
        CapsuleCollider collider = GetComponent<CapsuleCollider>(); 
        Transform colliderTra = collider.GetComponent<Transform>();


        direction = joystick.Direction;  
        directionX = direction.x; 
        directionY = direction.y; 
        

        if(directionY > (Math.Sqrt(3)/2) && (directionX > -1/2 || directionX < 1/2)){
            playerMovement.speed = playerMovement.speedInit;
            Vector3 moveDir = directionX * cameraTransform.right.normalized + directionY * cameraTransform.forward.normalized;
            colliderTra.position += moveDir * playerMovement.speed * Time.deltaTime;
            cameraTransform.transform.position = colliderTra.position + new Vector3(0,3,0);
        }

        
        if(directionY > 1/2 && ((directionX > -Math.Sqrt(3)/2) || (directionX < Math.Sqrt(3)/2))){
            playerMovement.speed = playerMovement.speedInit/2;
            Vector3 moveDir = directionX * cameraTransform.right.normalized + directionY * cameraTransform.forward.normalized;
            colliderTra.position += moveDir * playerMovement.speed * Time.deltaTime;
            cameraTransform.transform.position = colliderTra.position + new Vector3(0,3,0);
        }

        
        if(directionY < 0){
            playerMovement.speed = playerMovement.speedInit/3;
            Vector3 moveDir = directionX * cameraTransform.right.normalized + directionY * cameraTransform.forward.normalized;
            colliderTra.position += moveDir * playerMovement.speed * Time.deltaTime;
            cameraTransform.transform.position = colliderTra.position + new Vector3(0,3,0);
        }

        //if(!(directionX > (Math.Sqrt(2)/2) && directionY > (Math.Sqrt(2)/2)) && (directionX > (-Math.Sqrt(2)/2) && directionY > (Math.Sqrt(2)/2))){
        //    playerMovement.speed = 1f;
        //}


    }


}

