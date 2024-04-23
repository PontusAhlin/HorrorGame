using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCastTest : MonoBehaviour
{
    public List<GameObject> currentHitObjects = new List<GameObject>();

    public Camera camera;
    
    public float sphereRadius; 
    public float maxDistance;
    public float minDistance;

    private Vector3 sphereCastOffset;
    private Vector3 playerDirection;
    private float currentHitDistance;
    public LayerMask layerMask;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //Sets the players position and direction continuisly where the player looks 
        sphereCastOffset = transform.position + transform.forward * minDistance;
        playerDirection = transform.forward;
        //Clears the gameObject list each frame
        currentHitObjects.Clear();

        //This line gives us an array with everything our raycast sphere hits 
        RaycastHit[] hits = Physics.SphereCastAll(sphereCastOffset, sphereRadius, playerDirection, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);


        //This list where all of the objects the sphereCast can see  
        foreach (RaycastHit hit in hits){            
    
            //If we we see something with the monster tag we inspect it 
            if(hit.transform.gameObject.tag == "Monster"){
                RaycastHit hitMonster;
                
                //We look at the direction in which the player can see the monster
                Vector3 monsterHitDirection = Vector3.Normalize(hit.point - transform.position);  

                // Calculate distance between player and monster.
                float distanceBetween = Vector3.Distance(transform.position, hit.point);
                // Debug ray to see if we hit something between player and monster.
                Debug.DrawRay(transform.position, monsterHitDirection * (distanceBetween - 0.1f), Color.red);

                //This makes a new separate raycast towards the monster, if there is something colliding with the raycast it doesn't register the monster 
                if(Physics.Raycast(transform.position, monsterHitDirection, out hitMonster , distanceBetween - 0.1f) == false){
                    currentHitObjects.Add(hit.transform.gameObject);
                    print("SHIT A MONSTER");

                    //Here we should add the points 

                }
            }
        }
    }

    
    //Debugging by creating the raycast sphere 
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, sphereCastOffset + playerDirection * maxDistance);
        Gizmos.DrawWireSphere(sphereCastOffset + playerDirection * maxDistance , sphereRadius);
    }

}
