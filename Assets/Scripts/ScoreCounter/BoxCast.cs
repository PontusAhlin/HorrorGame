/**
    *   
    * This script connects to the main camera 
    * so that when you look at a monster you
    * generate a score. A prerequisite is
    * MonsterGenerateViewers.cs and PlayerScore.cs
    * which generates the score. This is mainly used for 
    * detection of monsters and viewer requests. 
    *
    * Authors: Pontus Åhlin, William Fridh, Sai Chintapalli
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BoxCast : MonoBehaviour
{
    /* //Added for debugging
    [Tooltip("List of objects which the raycast box detects ")]
    public List<GameObject> currentHitObjects = new List<GameObject>();
    */
    
    //Init of detection components
    public float maxDistance;
    [Tooltip("The 'offset' from the player forward which the sphere will start from  ")]
    public float minDistance;
    private Vector3 boxCastOffset;
    private Vector3 playerDirection;
    [Tooltip("DON'T TOUCH(BUT IF YOU DO SET IT BACK TO 'Default')")]
    [SerializeField] LayerMask layerMask;
    
    //BoxCast adjustments 
    public Quaternion boxOrientation;
    public Vector3 halfBox; 


    //Lists regarding the monsters 
    private List<MonsterGenerateViewers> monsterInFov = new List<MonsterGenerateViewers>();
    public List<GameObject> seenMonsters = new List<GameObject>();

    //References to other scripts
    RandomMonsterGeneration randomMonsterGeneration;
    MonsterGenerateViewers monsterViewer;


    //Random indexing for monsterRequests 
    private int ranReqIndex;

    //Timer for the monster requests 
    public float viewerRequestTime;


    void Start(){
        //Request time is initilized to 30 seconds
        viewerRequestTime = 60.0f;
    }


    void FixedUpdate(){
        //Sets the players position and direction continuisly where the player looks 
        boxCastOffset = transform.position + transform.forward * minDistance;
        playerDirection = transform.forward;
        boxOrientation = transform.rotation;

        //Updating the timer
        viewerRequestTime -= Time.deltaTime;

        //Handles the requests from viewers
        viewerRequest();


        /*
        //Clears the gameObject list each frame(Used for debugging)
            currentHitObjects.Clear();
        */
        
        //Essential to reset the in field of view to false for the monster gameobject when not looking at it  
        for(int i = 0; i < monsterInFov.Count; i++){
            monsterInFov[i].inFieldOfView = false;
        }


        //Gives us an array with everything our raycast box hits 
        RaycastHit[] hits = Physics.BoxCastAll(boxCastOffset, halfBox , playerDirection, boxOrientation , maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);

        //List where all of the objects the boxCast can see  
        foreach (RaycastHit hit in hits){     

            //GameObject of what currently hit in boxcast
            GameObject hitObject = hit.collider.gameObject;
            MonsterGenerateViewers monsterGenerates = hitObject.GetComponent<MonsterGenerateViewers>();
  

            //If not seen before, add monster to seen monsters list(used for random viewerRequest)
            if(!seenMonsters.Contains(hitObject) && hit.transform.tag == "Monster"){
                seenMonsters.Add(hitObject);
            }


            //seen and spawned monsters are checked so only their unqiue ID triggers pointed raycast. 
            if(hit.transform.tag == "Monster"){
                RaycastHit hitMonster;

                //Debugging 
                //print("tag " + hit.transform.gameObject.tag);

                //We look at the direction in which the player can see the monster
                Vector3 monsterHitDirection = Vector3.Normalize(hit.point - transform.position);  

                // Calculate distance between player and monster.
                float distanceBetween = Vector3.Distance(transform.position, hit.point);
                // Debug ray to see if we hit something between player and monster.
                Debug.DrawRay(transform.position, monsterHitDirection * (distanceBetween - 0.1f), Color.red);

                //Makes a new separate raycast towards the monster, if there is something colliding with the raycast it doesn't register the monster.
                //if statement means that we can see the monster
                if(Physics.Raycast(transform.position, monsterHitDirection, out hitMonster , distanceBetween - 0.1f) == false){

                    //Part of resetting the FOV of monsters
                    if(!monsterInFov.Contains(monsterGenerates)){
                        monsterInFov.Add(monsterGenerates);
                    }
                    monsterGenerates.inFieldOfView = true;

                    }

            }
        }
    }
    

    void viewerRequest(){
        //After a certain time a seen monster will reset their viewer multiplier and be requested by the chat. 
        if(viewerRequestTime < 1.0f && seenMonsters.Count > 0){
            ranReqIndex = Random.Range(0,seenMonsters.Count);
            MonsterGenerateViewers reqMonster = seenMonsters[ranReqIndex].GetComponent<MonsterGenerateViewers>();
            reqMonster.mult = 1.0f;     

            print("I want to see the " + "PUT IN COLOUR OF MONSTER" + " " + seenMonsters[ranReqIndex].gameObject.name);
            viewerRequestTime = 60.0f;
        }
        //If we haven't seen a monster we reset the request time.  
        if(viewerRequestTime < 0.5f){
            print("Cmooon find a monsteeeer");
            viewerRequestTime = 30.0f;
        }
    }
    
    //Debugging by creating the raycast box and line towards the box
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, boxCastOffset + playerDirection * maxDistance);
        Gizmos.DrawWireCube(boxCastOffset + playerDirection * maxDistance , halfBox/2);
    }



}