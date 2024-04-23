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
        currentHitDistance = maxDistance;        
        
        //Clears the gameObject list each frame
        currentHitObjects.Clear();

        //This line gives us an array with everything our raycast sphere hits 
        RaycastHit[] hits = Physics.SphereCastAll(sphereCastOffset, sphereRadius, playerDirection, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);

        foreach (RaycastHit hit in hits){            
            if(hit.transform.gameObject.tag == "Monster"){
                RaycastHit hitMonster;
                Vector3 monsterHitDirection = Vector3.Normalize(hit.point - transform.position);  
                //print(monsterHitDirection);
                if(Physics.Raycast(transform.position, monsterHitDirection, out hitMonster , hit.distance - 0.1f) == false){
                    currentHitObjects.Add(hit.transform.gameObject);
                    print("SHIT A MONSTER");
                    
                }
            }
        }
    }


    

        /**   
        * Adds a new gameobject to our list of current gameobjects our sphere raycast can see.
        * Checks if we see a monster in sight we do something.
        */
        /*
        foreach (RaycastHit hit in hits){
            currentHitObjects.Add(hit.transform.gameObject);
            if(hit.transform.gameObject.tag == "Monster"){
                print("SHEIT A MONSTER");
            }
        }
    }*/
}
