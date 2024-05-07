/**
    * GhostMonster.cs
    *
    * This script is used to control the ghost monster's movement and detection of the player.
    * The ghost monster will patrol the map and chase the player if they are within the detection range.
    * If the player is within the kill range, the player will die and the scene will change to the jumpscare scene.
    *
    * Autor(s): ??? & William Fridh
    */

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
    public float speed; // maximum speed of ghost
    private float pace; // current speed of ghost
    public Transform MonsterTransform; // transform of the monster

    public UnityEngine.Rendering.Volume MonsterVolume;
    public string JumpscareScene; // scene name of the jumpscare scene
    Vector3 movementDirection; // normalized position destPoint positon vector
    public float rotationSpeed; // speed of rotation for monster, rotationSpeed is the magnitude of rotationVector
    Vector3 rotationVector; // vector about which we rotate, magnitude is the speed of roation.
    Vector3 direction; // vector from monster transform position to destPoints
    int randomX; // random x coordinate for destination
    int randomY; // random y coordinate for destination
    Vector3 playerDirection; // vector from monster to player
    
    float rand; // initialize random object
    [SerializeField] LayerMask groundLayer, playerLayer;

    Vector3 destPoint; // point we are walking towards
    bool walkPointSet; // whether or not enemy has a destination to walk towards
    [SerializeField] float walkRange; //determines how far an enemy can move in a single walk between pauses
    [SerializeField] int pauseTimeRange; // variable determining range of time to stop between walks

    // Start is called before the first frame update
    void Start()
    {
        // sets randomMapHandler to the instance of randomMapHandler at runtime
        randomMapHandler = GameObject.Find("/RandomMapGeneration").GetComponent<RandomMapHandler>();
        // sets speed at runtime
        pace = speed;
        // sets playerObject to player camera at runtime
        GameObject playerObject = GameObject.Find("Character & Camera");
        playerTransform = playerObject.transform; // Reference to the player's transform
        // searches for a starting destination
        SearchForDest();
       
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is within the monster's detection range
        IsPlayerVisible();
        // Patrol controls the all movement of the monster
        Patrol();
        // updates direction to move
        direction = destPoint - transform.position;
        // updates rotation of monster
        Rotate(rotationSpeed, MonsterTransform.forward, direction);
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
    // Searches for a pair of x,y coordingates to set as destination.
    void SearchForDest()
    {
       
        // picks a random coordinate on the map plane
        // Uses the randomMapHandler object to determine the width and height of map
        randomX = UnityEngine.Random.Range(0, randomMapHandler.MapWidth-1);
        randomY = UnityEngine.Random.Range(0, randomMapHandler.MapHeight-1);
        
        // Until we find a destination on the map that is not empty, we keep looking for coordinates
        while (randomMapHandler.gridHandler[randomX,randomY] == RandomMapHandler.Grid.EMPTY){
            randomX = UnityEngine.Random.Range(0, randomMapHandler.MapWidth-1);
            randomY = UnityEngine.Random.Range(0, randomMapHandler.MapHeight-1);
        }
        walkPointSet = true;    // now walkPointSet is true. Meaning we have a destination.
        randomX = randomX * randomMapHandler.RoomSize; // scales the room coordinate to Unity coordinates
        randomY = randomY * randomMapHandler.RoomSize;
        
        // Converts the destination coordinate to a vector
        destPoint = new Vector3((float)randomX,0,(float)randomY); 
        
    }

    

    void IsPlayerVisible()
    {
        // Calculate vector from the monster to the player
        playerDirection = playerTransform.position - MonsterTransform.position;
        // Casts a ray from the monster in the direction of the player
        Ray ray = new Ray(MonsterTransform.position, playerDirection.normalized);

    // if the ray hits an object within the layersToHit
    // layersToHit includes the player layer and the navmesh layer
    // killRange is a public variable specifying proximity at which monster will kill player
    if (Physics.Raycast(ray, out RaycastHit killHit, killRange,layersToHit))
        {
            // Check if the ray hits the player which is labelled as "Character & Camera"
            if (killHit.collider.gameObject.name.Equals("Character & Camera")) {
                // Call player death function to store likes and viewers.
                Camera playerCamera = killHit.collider.gameObject.GetComponentInChildren<Camera>();
                PlayerScore playerScore = playerCamera.GetComponent<PlayerScore>();
                playerScore.Death();
                // Target the achievement holder and call it's hanlding function.
                GameObject achievementHolder = killHit.collider.gameObject.transform.parent.Find("AchievementHolder").gameObject;
                AchievementHandler(achievementHolder);
                // Change scene to jumpscare scene.
                ChangeScene(JumpscareScene);
            }
        }

        // huntRange is a public variable specfying proximity at which monster will start following player
        if (Physics.Raycast(ray, out RaycastHit huntHit, huntRange,layersToHit))
        {
            // Check if the ray hits the player
            if (huntHit.collider.gameObject.name.Equals("Character & Camera")) {
                // if player is hit by ray then set destPoint to player position
                destPoint = huntHit.point;
                walkPointSet = true;
                MonsterVolume.weight = 1.0f;
            }
            else
            {
                MonsterVolume.weight = 0.1f;
            }
        }
    }

    /**
        * Achievement Handler.
        *
        * This function is called when the player is killed by the monster.
        * It handles the achievements for the player related to the death.
        */
    void AchievementHandler(GameObject achievementHolder)
    {
        // Get component.
        AchievementAbstract AchievementFour = achievementHolder.transform.Find("AchievementFour").GetComponent<AchievementAbstract>();
        // Increase progress.
        AchievementFour.AddProgress(1);
    }

    void PlayerDeath()
    {
        // Perform actions to handle player death, such as displaying a game over screen, resetting the level, etc.
        Debug.Log("Player died!");
    }

    private void StopEnemy()
    {
        // pace determines magnitude of velocity of monster
        pace = 0;
    }

    private void StartEnemy()
    {
        // speed is a public variable determining maximum velocity of monster
        pace = speed;
    }

    // function for switching scenes
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    
    // This function moves the monster towards the Vector3 destPoint
    public void Travel() 
    {
        
        movementDirection = destPoint.normalized;
        MonsterTransform.position = Vector3.MoveTowards(MonsterTransform.position, destPoint, pace * Time.deltaTime);
        
    }

    // takes care of the rotation of the monster
    // rotationSpeed is radians/seconds
    // forward is the vector associated with blue arrow for monster
    // destdir is the desired direction
    public void Rotate(float rotationSpeed, Vector3 forward, Vector3 destdir)
    {
        // rotation is the vector about which we rotate
        rotationVector = new Vector3(0,rotationSpeed,0);
        // Quaternion is a measure of how much rotation there is between two vectors
        Quaternion rot = Quaternion.FromToRotation(forward, destdir);
        // here we pick the closest rotation (whether to turn left or right)
        if (rot.y > 0)
        {
            transform.Rotate(rotationVector * Time.deltaTime);
        }
        if (rot.y <= 0)
        {
            MonsterTransform.Rotate(-rotationVector * Time.deltaTime);
        }
    }
   

}
