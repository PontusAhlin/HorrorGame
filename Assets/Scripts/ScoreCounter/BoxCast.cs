/**
    * This script connects to the main camera 
    * so that when you look at a monster you
    * generate a score. A prerequisite is
    * MonsterGenerateViewers.cs and PlayerScore.cs
    * which generates the score. This is mainly used for 
    * detection of monsters and viewer requests. 
    *
    * Authors: Pontus Åhlin, William Fridh, Sai Chintapalli, Moritz Gruss
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
    [Tooltip("How far the boxcast goes")]
    public float maxDistance;
    [Tooltip("The 'offset' from the player forward which the box will start from")]
    public float minDistance;
    private Vector3 boxCastOffset;
    private Vector3 playerDirection;
    [Tooltip("DON'T TOUCH(BUT IF YOU DO SET IT BACK TO 'Default')")]
    [SerializeField] LayerMask layerMask;
    
    //BoxCast adjustments 
    [Tooltip("The orientation of the boxcast")]
    public Quaternion boxOrientation;
    [Tooltip("The width of the boxcast")]
    public Vector3 halfBox; 


    //Lists regarding the monsters 
    private List<MonsterGenerateViewers> monsterInFov = new List<MonsterGenerateViewers>();
    public List<GameObject> seenMonsters = new List<GameObject>();

    //References to other scripts
    RandomMonsterGeneration randomMonsterGeneration;
    MonsterGenerateViewers monsterViewer;
    //ChangeColour changeColour;        //To be added later
    ChatGeneration chatGeneration;
    PlayerScore playerScore;
    InGameInterface inGameInterface;

    //Random indexing for monsterRequests 
    private int ranReqIndex;

    //Timer for the monster requests 
    [Tooltip("How often viewers should request a monster")]
    public float viewerRequestTime;
    private float viewerRequestTimeInit;
    //Timer for how often the chat should appear
    [Tooltip("How often a viewer should send a message in chat")]
    public float chatTimeInterval;
    private float chatTimeIntervalInit;

    //Where viewer messages will be stored from ChatGeneration.cs 
    private string viewerMsg;
    //Bool that makes it so messages don't overlap
    private bool extraBreak;

    // Moritz
    // ChangeColour object containing the colour of the current "special "monster to follow for views
    // we instantiatie a class for each type of monster
    private GhostColors currentColorGhost;
    private MannequinColors currentColorMannequin;
    private ChargerColors currentColorCharger;


    // string obejct for holding substring of Monster name
    // before monster name was "Ghost(clone)" for example
    // we therefore need only the substring
    private string monsterName;


    void Start(){
        //Request time is initilized to 60 seconds
        viewerRequestTimeInit = viewerRequestTime;
        chatTimeIntervalInit = chatTimeInterval;
    }



    void FixedUpdate(){
        //Sets the players position and direction continuisly where the player looks 
        boxCastOffset = transform.position + transform.forward * minDistance;
        playerDirection = transform.forward;
        boxOrientation = transform.rotation;


        //Updating the timers
        viewerRequestTime -= Time.deltaTime;
        chatTimeInterval -= Time.deltaTime; 


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
        extraBreak = false;
        //Getting the viewercount from the PlayerScore.cs
        GameObject pScore = GameObject.Find("Main Camera");
        playerScore = pScore.GetComponent<PlayerScore>();   
        
        //After a certain time a seen monster will reset their viewer multiplier and be requested by the chat. 
        //If nothing is seen in set amount of time the chat will be bored.
        if(chatTimeInterval < 2.2f && playerScore.viewers != 0){

            //Goes through all of the monsters that's in the field of view, if that's true we send a message to the UI
            for(int i = 0; i < monsterInFov.Count; i++){
                if(monsterInFov[i].inFieldOfView == true || playerScore.viewers > 50){
                    getChat();
                    viewerMsg = chatGeneration.GenerateMessage("Positive");
                    inGameInterface.PrintMessage(viewerMsg,"baseline_person_white_icon");
                    chatTimeInterval = chatTimeIntervalInit/2;
                    extraBreak = true;
                    break;
                }
            }
            
            //The case if the player doesn't see a monster 
            if(extraBreak == false && playerScore.viewers < 50 && playerScore.viewers != 0){
                getChat();
                viewerMsg = chatGeneration.GenerateMessage("Negative");
                inGameInterface.PrintMessage(viewerMsg,"baseline_person_white_icon");
                //Resetting the timers
                chatTimeInterval = chatTimeIntervalInit;
            }
        }
            
        if(viewerRequestTime < 2f && seenMonsters.Count > 0 && chatTimeInterval < 4f && playerScore.viewers != 0){
            
            

            //Randomly selects a seen monster to be requested and resets the multipler of it,
            ranReqIndex = Random.Range(0,seenMonsters.Count);
            MonsterGenerateViewers reqMonster = seenMonsters[ranReqIndex].GetComponent<MonsterGenerateViewers>();
            reqMonster.mult = 1.0f;     

            // Moritz
            // gets ChangeColour object from monster objects
            currentColorGhost = seenMonsters[ranReqIndex].gameObject.GetComponent<GhostColors>();
            currentColorMannequin = seenMonsters[ranReqIndex].gameObject.GetComponent<MannequinColors>();
            currentColorCharger = seenMonsters[ranReqIndex].gameObject.GetComponent<ChargerColors>();

            // places monster object name in auxilliary string
            monsterName = seenMonsters[ranReqIndex].gameObject.name;
            // extracts substring, exluding "(Clone)" part
            monsterName = monsterName.Remove(monsterName.Length - 7);
            
            getChat();
            //Color is to be replaced with the color of the monster to finalize viewer message

            if (currentColorGhost != null)
            {
                viewerMsg = ("I want to see the " + currentColorGhost.CurrentColor + " " + monsterName);
            }
            else if (currentColorMannequin != null) 
            {
                viewerMsg = ("I want to see the " + currentColorMannequin.CurrentColor + " " + monsterName);
            }
            else 
            {
                viewerMsg = ("I want to see the " + currentColorCharger.CurrentColor + " " + monsterName);
            }

            currentColorGhost = null;
            currentColorMannequin = null;
            currentColorCharger = null;
            
            
            inGameInterface.PrintMessage(viewerMsg,"baseline_person_white_icon", Color.red);
            //Resets the timers
            viewerRequestTime = viewerRequestTimeInit;
            chatTimeInterval = chatTimeIntervalInit;
        }
    }
    


    //Debugging by creating the raycast box and line towards the box
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, boxCastOffset + playerDirection * maxDistance);
        Gizmos.DrawWireCube(boxCastOffset + playerDirection * maxDistance , halfBox/2);
    }

    /*
        * Gets the chat reference from ChatManager and the ingame interface
    */
    private void getChat(){
        GameObject uiElem = GameObject.Find("In Game Interface");
        inGameInterface = uiElem.GetComponent<InGameInterface>(); 
        GameObject chatGenerationGameObject = GameObject.Find("ChatManager");
        chatGeneration = chatGenerationGameObject.GetComponent<ChatGeneration>();
    }



}