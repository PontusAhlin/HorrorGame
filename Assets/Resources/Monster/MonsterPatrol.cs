using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class enemyAiControl : MonoBehaviour
{

    static private GameObject playerObject;
    //GameObject player;
    public Transform playerTransform; // Reference to the player's transform
    public float killRange = 10f; // Detection range of the monster
    public float huntRange = 100f; 
    public LayerMask layersToHit; // Layer mask to detect obstacles between the monster and the player


    NavMeshAgent agent; // initialize agent object referring to scripted object

    float rand; // initialize random object

    [SerializeField] LayerMask groundLayer, playerLayer;

    [SerializeField] private Animator Animator;
    private const string isMoving = "isMoving";
    

    Vector3 destPoint; // point we are walking towards
    bool walkPointSet; // whether or not enemy has a destination to walk towards
    [SerializeField] float walkRange; //determines how far an enemy can move in a single walk between pauses

    [SerializeField] int pauseTimeRange; // variable determining range of time to stop between walks

    NavMeshHit hit; // initializes variable in which the chosen destination will be stored. 

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // agent object corresponding to object that script is attributed to.
        GameObject playerObject = GameObject.Find("Character & Camera");
        playerTransform = playerObject.transform; // Reference to the player's transform
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is within the monster's detection range
        //Debug.Log(Vector3.Distance(transform.position, player.position));
        
        IsPlayerVisible();

        Patrol();

        Animator.SetBool(isMoving, agent.velocity.magnitude < 0.01f);
    }

    void Patrol()
    {
        
        if (!walkPointSet) SearchForDest(); // if there is no walk destination, find one.
        
        /* if distance between current position to destination is less than 2
         then destination has been reached */
        if(Vector3.Distance(transform.position, destPoint) < 2)
        {
            StopEnemy(); // stop agent 
            rand = Random.Range(0,pauseTimeRange); // picks a time in the stop range (serialized as StopRange) to pause for
            Invoke("StartEnemy",rand); // delays the call of StartEnemy method by rand seconds
            walkPointSet = false; // when walk point set is false, SearchForDest() will be called
            // in the enxt iteration of Patrol()
        }
        else
        {
            goToTarget();
        }
        
    }

    void SearchForDest()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRange; /* picks a random 3D coordinate in 
        in the range of the sphere (serialized as Range) */ 

        randomDirection += transform.position; // add the current position to randomDirection
        NavMesh.SamplePosition(randomDirection, out hit, walkRange, 1); /* find closest point on the navmesh to randomDirection
         store this in hit */
        destPoint = hit.position; // set destination to hit
        walkPointSet = true; // now walkPointSet is true.

    }

    private void StopEnemy()
    {
        agent.isStopped = true;
    }

    private void StartEnemy()
    {
        agent.isStopped = false;
    }

    private void goToTarget()
    {
        agent.SetDestination(destPoint); // agent.SetDestination provided by Unity engine
        
    }

    void IsPlayerVisible()
    {
        // Calculate direction from the monster to the player
        Vector3 direction = playerTransform.position - transform.position;
        Ray ray = new Ray(transform.position, direction.normalized);

        // Cast a ray from the monster towards the player
    
    if (Physics.Raycast(ray, out RaycastHit killHit, killRange,layersToHit))
        {
            // Check if the ray hits the player
            if (killHit.collider.gameObject.name.Equals("Character & Camera")) {
                ChangeScene("JumpScare");
            }
            Debug.Log(killHit.collider.gameObject.name + " was hit!");

        }

    if (Physics.Raycast(ray, out RaycastHit huntHit, huntRange,layersToHit))
        {
            // Check if the ray hits the player
            if (huntHit.collider.gameObject.name.Equals("Character & Camera")) {
                destPoint = huntHit.point;
                walkPointSet = true;

            }
            Debug.Log(huntHit.collider.gameObject.name + " was hit!");

        }
    }

    void PlayerDeath()
    {
        // Perform actions to handle player death, such as displaying a game over screen, resetting the level, etc.
        Debug.Log("Player died!");
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
   

}
