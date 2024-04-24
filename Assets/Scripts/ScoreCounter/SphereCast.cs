using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCastTest : MonoBehaviour
{
    /* //Added for debugging
    [Tooltip("List of objects which the raycast sphere detects ")]
    public List<GameObject> currentHitObjects = new List<GameObject>();
    */
    
    [Tooltip("Connect this to your main camera")]
    public Camera camera;

    //Init of detection components
    [Tooltip("Radius of the raycasted sphere")]
    public float sphereRadius; 
    [Tooltip("How far the raycast sphere should go")]
    public float maxDistance;
    [Tooltip("The 'offset' from the player forward which the sphere will start from  ")]
    public float minDistance;
    private Vector3 sphereCastOffset;
    private Vector3 playerDirection;
    private float currentHitDistance;
    [Tooltip("DON'T TOUCH(BUT IF YOU DO SET IT BACK TO 'Default')")]
    public LayerMask layerMask;

    private float viewerAddAmnt; 
    private float viewerAddAmntTotal;
    private float viewerAddAmntMonster;
    private float viewerAddAmntSpecial;

    // Update is called once per frame
    void FixedUpdate(){
        //Sets the players position and direction continuisly where the player looks 
        sphereCastOffset = transform.position + transform.forward * minDistance;
        playerDirection = transform.forward;
        
        //Resets if the monster isn't in the field of view
        MonsterGenerateViewers.inFieldOfView = false;
        
        MonsterGenerateViewers.viewerAddAmount = viewerAddAmntTotal; 

        /*
        // Clears the gameObject list each frame(Used for debugging)
            currentHitObjects.Clear();
        */
        

        //Gives us an array with everything our raycast sphere hits 
        RaycastHit[] hits = Physics.SphereCastAll(sphereCastOffset, sphereRadius, playerDirection, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);


        //List where all of the objects the sphereCast can see  
        foreach (RaycastHit hit in hits){            
    
            //If we we see something with the monster tag we inspect it 
            if(hit.transform.gameObject.tag == "Monster" || hit.transform.gameObject.tag == "SpecialMonster"){
                RaycastHit hitMonster;
                
                //We look at the direction in which the player can see the monster
                Vector3 monsterHitDirection = Vector3.Normalize(hit.point - transform.position);  

                // Calculate distance between player and monster.
                float distanceBetween = Vector3.Distance(transform.position, hit.point);
                // Debug ray to see if we hit something between player and monster.
                Debug.DrawRay(transform.position, monsterHitDirection * (distanceBetween - 0.1f), Color.red);

                //Makes a new separate raycast towards the monster, if there is something colliding with the raycast it doesn't register the monster.
                //if statement means that we can see the monster
                if(Physics.Raycast(transform.position, monsterHitDirection, out hitMonster , distanceBetween - 0.1f) == false){
                    /* //Used for debugging
                        currentHitObjects.Add(hit.transform.gameObject);
                    */

                    //Switch statement checks the tag of the current monster and deals with special cases
                    switch(hit.transform.gameObject.tag)
                    {
                        case "Monster":
                            MonsterGenerateViewers.inFieldOfView = true;
                            viewerAddAmntMonster = 1.0f;
                        break;

                        //Add special monster functionality
                        case "SpecialMonster":
                            MonsterGenerateViewers.inFieldOfView = true;
                            viewerAddAmntSpecial = 5.0f;
                        break;
                    }
                }

                viewerAddAmntTotal = viewerAddAmntMonster + viewerAddAmntSpecial;
                MonsterGenerateViewers.viewerAddAmount = viewerAddAmntTotal;     
                viewerAddAmntTotal = 0.0f;   
            }
        
        }    
    }

    

    
    //Debugging by creating the raycast sphere and line towards the sphere
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, sphereCastOffset + playerDirection * maxDistance);
        Gizmos.DrawWireSphere(sphereCastOffset + playerDirection * maxDistance , sphereRadius);
    }

}
