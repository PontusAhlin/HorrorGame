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
        //The positions of the player and the monster 
        playerPos = transform.position;
        monsterPos = transform.position;

        //Check if the 
        if (Physics.Linecast(playerPos, monsterPos) && monsterPos.magnitude < 10) {
            print("There is something in front of the object!");
        }



        /**
            Check angle between the middle ray infront of camera and the field of view.
            If the monster is in between that angle, it's supposed to be detected
        
        */


        //float FOV = mainCamera.fieldOfView;
        

        //y is the forward direction as such (0,1,0)
        float angleLeft = Vector3.Angle(new Vector3(-1,1,0).normalized,new Vector3(0,1,0));
        float angleRight = Vector3.Angle(new Vector3(1,1,0).normalized,new Vector3(0,1,0));

        
        RaycastHit hit; 

        if (Physics.Linecast(playerPos, playerPos + monsterPos, out hit)) {
            Vector3 hitNormal = hit.normal;
            float playerMonsterAngle = Vector3.Angle(monsterPos,hitNormal);
            print("Angle between player and monster" + playerMonsterAngle); 
        }



        print("Angle between left and forward" + angleLeft);
        print("Angle between right and forward" + angleRight);








    }




}
