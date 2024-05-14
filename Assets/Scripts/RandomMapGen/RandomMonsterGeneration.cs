/**
    * Random Monster Generation.
    *
    * This script is used for randomly generating monsters in the game.
    * It can be used to spawn a specific number of monsters or spawn
    * a random number of monsters. It also makes sure the monsters don't
    * spawn too close to the player.
    *
    * If the script fails at findind a suitable spawn location for the monster,
    * it will print an error message to the console and stop the coroutine.
    *
    * TODO:
    * - Make it so that all coordinated are stored as vectors or arrays.
    * - Add a check to make sure the monsters don't spawn too close to each other.
    *
    * Author(s): Sai Chintapalli, Pontus Åhlin, William Fridh
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMonsterGeneration : MonoBehaviour
{
    public GameObject Player;

    [Tooltip("TRUE = spawn one of each monster first before randomizing. FALSE = truly random spawns")]
    public bool SpawnOneOfEach;

    [Tooltip("monsters in this list will be randomly picked to spawn")]
    public List<GameObject> MonsterPrefabs = new List<GameObject>();

    [Tooltip("time delay between monsters spawned after the first")]
    public int timeSeconds = 3;

    [Tooltip("How many monsters should be generated in total")]
    public int MonsterAmount = 3;

    [Tooltip("how much to wait before the FIRST monster spawns")]
    public int firstMonsterDelay = 5;

    [Tooltip("Minimum distance between player and monster (declared in room size).")]
    [SerializeField] int minRoomDistanceBetweenPlayerAndMonster = 4;

    public int CurrentMonsterAmount = 0;

    [Tooltip("Insert the RandomMapHandler script here pls")]
    private int xCoord, zCoord;

    private bool firstMonsterSpawned = false;
    public RandomMapHandler RandomMapScript;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        if (minRoomDistanceBetweenPlayerAndMonster < 1)
            minRoomDistanceBetweenPlayerAndMonster = 1;
    }

    // Start is called before the first frame update.
    void Start()
    {
        // Start the coroutine we define below named ExampleCoroutine.
        if(CurrentMonsterAmount < MonsterAmount)
            StartCoroutine(GenerateMonster(0)); 
    }

    /**
        * Is Player And Monster Same Room.
        *
        * Check if the player is in the same room as the monster.
        */
    private bool IsPlayerAndMonsterSameRoom(int xCoord, int zCoord)
    {
        return (
            xCoord == Mathf.RoundToInt(Player.transform.position.x/RandomMapScript.RoomSize) && 
            zCoord == Mathf.RoundToInt(Player.transform.position.z/RandomMapScript.RoomSize)
        );
    }

    /**
        * Generate Random Coordinates.
        *
        * Generate random coordinates for the monster to spawn at.
        */
    private int[] GenerateRandomCoordinates()
    {
        RandomMapHandler randomMapHandler = gameObject.GetComponent<RandomMapHandler>();
        xCoord = Random.Range(0, randomMapHandler.MapWidth - 1);
        zCoord = Random.Range(0, randomMapHandler.MapHeight - 1);
        return new int[] {xCoord, zCoord};
    }

    /**
        * GenerateMonster.
        *
        * This function is used to generate monsters in the game.
        * It is called by the Start function and is used to spawn
        * monsters in the game.
    */
    IEnumerator GenerateMonster(int monsterIndex){
        // Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        // Yield on a new YieldInstruction that waits for 5 seconds.
        if (firstMonsterSpawned)
            yield return new WaitForSeconds(timeSeconds);
        else
        {
            firstMonsterSpawned = true;
            yield return new WaitForSeconds(firstMonsterDelay);
        }

        // After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);

        // Generate a random x and z coordinate for the monster to spawn at.
        // We subtract 1 from the MapWidth and MapHeight because the array is 0 indexed.
        RandomMapHandler randomMapHandler = gameObject.GetComponent<RandomMapHandler>();
        int[] randomCoordinates = GenerateRandomCoordinates();
        xCoord = randomCoordinates[0];
        zCoord = randomCoordinates[1];

        Debug.Log("=================");
        Debug.Log(Vector3.Distance(new Vector3(xCoord, 0, zCoord), Player.transform.position));
        Debug.Log(randomMapHandler.RoomSize*minRoomDistanceBetweenPlayerAndMonster);

        int maxTries = 100;
        while (
            maxTries-- > 0 &&
            (
                // If the grid is empty...
                RandomMapScript.gridHandler[xCoord,zCoord] == RandomMapHandler.Grid.EMPTY ||
                // If the player is in the same room as the monster...
                IsPlayerAndMonsterSameRoom(xCoord, zCoord) ||
                // If the distance between player and monster is less than double the room size...
                Vector3.Distance(new Vector3(xCoord, 0, zCoord), Player.transform.position) < randomMapHandler.RoomSize*minRoomDistanceBetweenPlayerAndMonster
            )
        ) {
            // Generate a new random x and z coordinate for the monster to spawn at.
            // We subtract 1 from the MapWidth and MapHeight because the array is 0 indexed.
            randomCoordinates = GenerateRandomCoordinates();
            xCoord = randomCoordinates[0];
            zCoord = randomCoordinates[1];
        }
        if (maxTries <= 0)
        {
            Debug.Log("RandomMonsterGeneration: Failed to find a suitable spawn location for the monster.");
            yield break;
        }

        // Multiply the x and z coordinates by the room size to get the actual position.
        xCoord = xCoord * randomMapHandler.RoomSize;
        zCoord = zCoord * randomMapHandler.RoomSize;

        if (SpawnOneOfEach) // Spawn one of each monster first before randomizing.
        {
            if (monsterIndex == (MonsterPrefabs.Count-1))
                SpawnOneOfEach = false;
            Instantiate(MonsterPrefabs[monsterIndex], new Vector3(xCoord, 2, zCoord), Quaternion.identity);
            CurrentMonsterAmount++;
            if(CurrentMonsterAmount < MonsterAmount)
                StartCoroutine(GenerateMonster(monsterIndex+1));
        }
        else // Truly random spawns.
        {
            Instantiate(MonsterPrefabs[Random.Range(0,MonsterPrefabs.Count)], new Vector3(xCoord, 2, zCoord), Quaternion.identity);
            CurrentMonsterAmount++;
            if(CurrentMonsterAmount < MonsterAmount)
                StartCoroutine(GenerateMonster(0));
        }
    }
}

