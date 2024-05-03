using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class GhostMonster : MonoBehaviour
{
    private RandomMapHandler randomMapHandler;
    static private GameObject playerObject;
    //GameObject player;
    public Transform playerTransform; // Reference to the player's transform
    public float killRange = 10f; // Detection range of the monster
    public float huntRange = 100f; 
    public LayerMask layersToHit; // Layer mask to detect obstacles between the monster and the player
    public float speed;
    private float pace;
    public Transform MonsterTransform;
    Vector3 movementDirection;
    public float rotationSpeed;
    Vector3 rotation;
    Vector3 direction;
    int randomX;
    int randomY;
    Vector3 playerDirection;

    public enum Grid {
        EMPTY
    }
    


    

    float rand; // initialize random object

    [SerializeField] LayerMask groundLayer, playerLayer;

    [SerializeField] private Animator Animator;
    private const string isMoving = "isMoving";
    

    Vector3 destPoint; // point we are walking towards
    bool walkPointSet; // whether or not enemy has a destination to walk towards
    [SerializeField] float walkRange; //determines how far an enemy can move in a single walk between pauses

    [SerializeField] int pauseTimeRange; // variable determining range of time to stop between walks

    

    // Start is called before the first frame update
    void Start()
    {
        randomMapHandler = GameObject.Find("/RandomMapGeneration").GetComponent<RandomMapHandler>();
        Debug.Log(randomMapHandler);
        pace = speed;
       
        GameObject playerObject = GameObject.Find("Character & Camera");
        playerTransform = playerObject.transform; // Reference to the player's transform
        SearchForDest();
       
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is within the monster's detection range
        //Debug.Log(Vector3.Distance(transform.position, player.position));
        
        IsPlayerVisible();

        Patrol();
        direction = destPoint - transform.position;
        Debug.Log(direction);
        Rotate(rotationSpeed, MonsterTransform.forward, direction);

        //Animator.SetBool(isMoving, agent.velocity.magnitude < 0.01f);
    }

    void Patrol()
    {
        
        if (!walkPointSet) SearchForDest(); // if there is no walk destination, find one.
        
        /* if distance between current position to destination is less than 2
         then destination has been reached */
        if(Vector3.Distance(MonsterTransform.position, destPoint) < 2)
        {
            
            StopEnemy(); // stop agent 
            rand = Random.Range(0,pauseTimeRange); // picks a time in the stop range (serialized as StopRange) to pause for
            Invoke("StartEnemy",rand); // delays the call of StartEnemy method by rand seconds
            walkPointSet = false; // when walk point set is false, SearchForDest() will be called
            // in the enxt iteration of Patrol()
        }
        else
        {
            Travel();
            //Rotate(rotationSpeed, MonsterTransform.forward, movementDirection);
        }
        
    }

    void SearchForDest()
    {

        randomX = UnityEngine.Random.Range(0, randomMapHandler.MapWidth-1);
        randomY = UnityEngine.Random.Range(0, randomMapHandler.MapHeight-1);
        
        while (randomMapHandler.gridHandler[randomX,randomY] == RandomMapHandler.Grid.EMPTY){
            randomX = UnityEngine.Random.Range(0, randomMapHandler.MapWidth-1);
            randomY = UnityEngine.Random.Range(0, randomMapHandler.MapHeight-1);
        }
        walkPointSet = true;    // now walkPointSet is true.
        randomX = randomX * randomMapHandler.RoomSize;
        randomY = randomY * randomMapHandler.RoomSize;
        
        destPoint = new Vector3((float)randomX,0,(float)randomY);
        Debug.Log(destPoint);
    }

    

    void IsPlayerVisible()
    {
        // Calculate direction from the monster to the player
        playerDirection = playerTransform.position - MonsterTransform.position;
        Ray ray = new Ray(MonsterTransform.position, playerDirection.normalized);

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

    private void StopEnemy()
    {
        pace = 0;
    }

    private void StartEnemy()
    {
        pace = speed;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    

    public void Travel() 
    {
        // destPoint
        
        movementDirection = destPoint.normalized;
        MonsterTransform.position = Vector3.MoveTowards(MonsterTransform.position, destPoint, pace * Time.deltaTime);
        
    }

    public void Rotate(float rotationSpeed, Vector3 forward, Vector3 destdir)
    {
        rotation = new Vector3(0,rotationSpeed,0);
        Quaternion rot = Quaternion.FromToRotation(forward, destdir);
        if (rot.y > 0)
        {
            transform.Rotate(rotation * Time.deltaTime);
        }

        if (rot.y <= 0)
        {
            MonsterTransform.Rotate(-rotation * Time.deltaTime);
        }
    }
   

}
