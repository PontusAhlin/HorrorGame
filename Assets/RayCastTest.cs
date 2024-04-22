using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTest : MonoBehaviour
{

    public bool seeMonster;
    public string monsterType; 

    //Initilizing the start and endpoints
    public Vector3 playerPos;
    public Vector3 monsterPos;

    //Initilizing the player and monster fields
    public Camera mainCamera; 
    public GameObject monster;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //The positions of the player and the monster(Ball should be monster) 
        playerPos = mainCamera.transform.position;
        monsterPos = monster.transform.position;

        //Check if the 
        if (Physics.Linecast(playerPos, monsterPos) && monsterPos.magnitude < 10) && CheckAngle(playerPos, monsterPos) {
            print("There is something in front of the object!");
        }
    }

    void CheckAngle(transfrom.position playerPos, transfrom.position monsterPos){


        /**
            Check angle between the middle ray infront of camera and the field of view.
            If the monster is in between that angle, it's supposed to be detected
        
        */


        //float FOV = mainCamera.fieldOfView;
        
        Vector3 fovVectorLeft = transform.TransformDirection(,0,0) //Init this to 45 degrees to make it simpler. 
        Vector3 fovVectorRight = transform.TransformDirection(, ,0) //Init this to 45 degrees to make it simpler. 
        Vector3 infrontPlayer = transform.TransformDirection(Vector3.forward);

        




    }



}
