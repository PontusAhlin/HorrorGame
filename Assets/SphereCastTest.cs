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

    private Vector3 playerOrigin;
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
        
        playerOrigin = transform.position + transform.forward * minDistance;
        playerDirection = transform.forward;

        currentHitDistance = maxDistance;        
        currentHitObjects.Clear();

        //This line gives us an array with everything our raycast sphere hits 
        RaycastHit[] hits = Physics.SphereCastAll(playerOrigin, sphereRadius, playerDirection, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);

        /**   
        * Adds a new gameobject to our list of current gameobjects our sphere raycast can see.
        * Checks if we see a monster in sight we do something.
        */
        foreach (RaycastHit hit in hits){
            currentHitObjects.Add(hit.transform.gameObject);
            if(hit.transform.gameObject.tag == "Monster"){
                print("SHEIT A MONSTER");
            }
        }


    }
}
