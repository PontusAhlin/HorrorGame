using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
By: ALIN
but really by this video
https://www.youtube.com/watch?v=6B7yOnqpK_Y
and this github repo
https://github.com/GarnetKane99/RandomWalkerAlgo_YT

what this does is generate a grid,
spawn these "walker" objects that walk around the grid and place rooms where they walk,
and then i needa code wtf it does with the grid
because this was setup to output to a tilemap but that's for 2d

YOU WANT TO WORK IN DrawRoom TO DRAW STUFF!!
TO MODIFY RANDOM GENERATION PARAMETERS, DO IT FROM THE INSPECTOR, ON THE OBJECT WITH THIS SCRIPT!
*/

public class TileGenWalker : MonoBehaviour
{
    [Tooltip("list of all the rooms that have 1 way out, and are 1x1")]
    public List<GameObject> Small1Way = new List<GameObject>();
    [Tooltip("list of all the rooms that have 2 way out corner style, and are 1x1")]
    public List<GameObject> Small2WayCorner = new List<GameObject>();
    [Tooltip("list of all the rooms that have 2 ways out corridor style, and are 1x1")]
    public List<GameObject> Small2WayCorridor = new List<GameObject>();

    [Tooltip("list of all the rooms that have 3 ways out, and are 1x1")]
    public List<GameObject> Small3Way = new List<GameObject>();
    [Tooltip("list of all the rooms that have 4 ways out, and are 1x1")]
    public List<GameObject> Small4Way = new List<GameObject>();
    public enum Grid
    {
        TWO_TWO,
        ONE_TWO,

        ONE_ONE,
        EMPTY

    }

    //Variables
    public Grid[,] gridHandler;
    public List<TileGenWalkerObject> Walkers; 
    [Tooltip("total map width in cells")]
    public int MapWidth = 30; //map size 
    [Tooltip("total map height in cells")]
    public int MapHeight = 30;
    [Tooltip("this should match the EXACT SIZE OF A 1X1 ROOM. this is how much we distance stuff as well, and we set our rooms to 50 as default!!!")]
    public int RoomSize = 5; // THIS IS HOW BIG ONE "SQUARE" ROOM OF 1x1 IS IN UNITY UNITS!!
    [Tooltip("more walkers - more uhhh...agents walking around generating the map. imagine you have ants on a piece of paper that u dunked in ink. wherever the ant walks, the map generates. this value sets the amount of ants u drop down")]
    public int MaximumWalkers = 10;
    [Tooltip("not relevant, just shows in editor how many tiles have been made, for debugging")]
    public int TileCount = default;
    [Tooltip("(0 -> 1), percentage of the map's total cells to be filled until it's done")]
    public float FillPercentage = 0.4f;
    [Tooltip("(0 -> 1), decides how chaotic the random gen is. lower = straighter rooms")]
    public float Randomness = 0.5f;
    [Tooltip("float value, but this should be like ZERO because all it does is delay the thing and lag!!")]
    public float WaitTime = 0.05f;
    [Tooltip("(0 -> 1), this decides the percentage chance that 2x2 rooms will be generated when possible")]
    public float TwoByTwoChance = 0.5f;
    [Tooltip("(0 -> 1), this decides the percentage chance that 1x2 rooms will be generated when possible")]
    public float OneByTwoChance = 0.5f;
    void Start()
    {
        InitializeGrid();
    }
    //this is the function that is called at coordinates X & Y 
    /* DrawRoom(coord, coord, isNorthOpen, isEastOpen, .....)




    */
    void DrawRoom(int x, int y, bool north, bool east, bool south, bool west)
    {
        //Debug.Log("drawn room at (" + x + "," + y + ")");

        switch (north, east, south, west) //THIS ENTIRE THING HANDLES DRAWING WHAT KIND OF ROOM, WHERE
        {   
            //SHOULDNT HAPPEN
            case (false, false, false, false):
            {
                Debug.Log("ROOM WITH NO EXITS WAS DRAWN");
                break;
            }
            //1 WAY
            case (true, false, false, false): //N
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small1Way.Count);
                Instantiate(Small1Way[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,0,0));
                break;
            }
            case (false, true, false, false): //E
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small1Way.Count);
                Instantiate(Small1Way[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,90f,0));
                break;
            }
            case (false, false, true, false): //S
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small1Way.Count);
                Instantiate(Small1Way[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,180f,0));
                break;
            }
            case (false, false, false, true): //W
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small1Way.Count);
                Instantiate(Small1Way[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,270f,0));
                break;
            }
            //2 WAY CORRIDORS
            case (true, false, true, false): //NS
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small2WayCorridor.Count);
                Instantiate(Small2WayCorridor[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,0,0));
                break;
            }
            case (false, true, false, true): //EW
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small2WayCorridor.Count);
                Instantiate(Small2WayCorridor[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,90f,0));
                break;
            }
            //2 WAY CORNERS
            case (true, true, false, false): //NE
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small2WayCorner.Count);
                Instantiate(Small2WayCorner[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,0,0));
                break;
            }
            case (false, true, true, false): //SE
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small2WayCorner.Count);
                Instantiate(Small2WayCorner[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,90f,0));
                break;
            }
            case (false, false, true, true): //SW
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small2WayCorner.Count);
                Instantiate(Small2WayCorner[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,180f,0));
                break;
            }
            case (true, false, false, true): //NW
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small2WayCorner.Count);
                Instantiate(Small2WayCorner[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,270f,0));
                break;
            }
            //3 WAY
            case (true, true, true, false): //NES
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small3Way.Count);
                Instantiate(Small3Way[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,0,0));
                break;
            }
            case (false, true, true, true): //SEW
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small3Way.Count);
                Instantiate(Small3Way[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,90f,0));
                break;
            }
            case (true, false, true, true): //NWS
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small3Way.Count);
                Instantiate(Small3Way[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,180f,0));
                break;
            }
            case (true, true, false, true): //NEW
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small3Way.Count);
                Instantiate(Small3Way[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,270f,0));
                break;
            }
            //4WAY
            default:
            {
                int prefabIndex = UnityEngine.Random.Range(0,Small4Way.Count);
                Instantiate(Small4Way[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.Euler(0,0,0));
                break;
            }
        }
    }

    void InitializeGrid()
    {
        gridHandler = new Grid[MapWidth, MapHeight];

        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                gridHandler[x, y] = Grid.EMPTY;
            }
        }

        Walkers = new List<TileGenWalkerObject>();

        Vector3Int TileCenter = new Vector3Int(gridHandler.GetLength(0) / 2, gridHandler.GetLength(1) / 2, 0);

        TileGenWalkerObject curWalker = new TileGenWalkerObject(new Vector2(TileCenter.x, TileCenter.y), GetDirection(), Randomness);
        gridHandler[TileCenter.x, TileCenter.y] = Grid.ONE_ONE;
        Walkers.Add(curWalker);

        TileCount++;

        StartCoroutine(CreateFloors());
    }

    Vector2 GetDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    IEnumerator CreateFloors()
    {
        while ((float)TileCount / (float)gridHandler.Length < FillPercentage)
        {
            Debug.Log("CreateFloors RAN");
            bool hasCreatedONE_ONE = false;
            foreach (TileGenWalkerObject curWalker in Walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);

                if (gridHandler[curPos.x, curPos.y] != Grid.ONE_ONE)
                {
                    TileCount++;
                    gridHandler[curPos.x, curPos.y] = Grid.ONE_ONE;
                    hasCreatedONE_ONE = true;
                }
            }

            //Walker Methods
            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();

            if (hasCreatedONE_ONE)
            {
                yield return new WaitForSeconds(WaitTime);
            }
        }

        /*
          THIS IS WHERE YOU SHOULD WRITE CODE THAT CARES ABOUT THE MAP
          LIKE
          THIS IS THE POINT WHERE THE MAP IS FULL OF Grid.EMPTY or GRID.ONE_ONE
        */

        for (int x = 0; x < MapWidth; x++) //HANDLING IF THERE ARE ANY ADJACENT GRID STUFFS
            for (int y = 0; y < MapHeight; y++)
            {
                if (gridHandler[x, y] == Grid.ONE_ONE)
                {
                    bool north = false, east = false, south = false, west = false;  //variables to hand to
                                                                                    //DrawRoom to select the right prefab
                                                                                    //with n/e/s/w entrances
                    // CHECKING NORTH
                    try
                    {
                        if (gridHandler[x,y+1] == Grid.ONE_ONE)
                            north = true;
                    }
                    catch (Exception) {}
                    // CHECKING EAST
                    try
                    {
                        if (gridHandler[x+1,y] == Grid.ONE_ONE)
                            east = true;
                    }
                    catch (Exception) {}
                    // CHECKING SOUTH
                    try
                    {
                        if (gridHandler[x,y-1] == Grid.ONE_ONE)
                            south = true;
                    }
                    catch (Exception) {}
                    // CHECKING WEST
                    try
                    {
                        if (gridHandler[x-1,y] == Grid.ONE_ONE)
                            west = true;
                    }
                    catch (Exception) {}
                    
                    DrawRoom(x,y,north,east,south,west);
                }
                
            }
    }

    void ChanceToRemove()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count > 1)
            {
                Walkers.RemoveAt(i);
                break;
            }
        }
    }

    void ChanceToRedirect()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange)
            {
                TileGenWalkerObject curWalker = Walkers[i];
                curWalker.Direction = GetDirection();
                Walkers[i] = curWalker;
            }
        }
    }

    void ChanceToCreate()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count < MaximumWalkers)
            {
                Vector2 newDirection = GetDirection();
                Vector2 newPosition = Walkers[i].Position;

                TileGenWalkerObject newWalker = new TileGenWalkerObject(newPosition, newDirection, Randomness);
                Walkers.Add(newWalker);
            }
        }
    }

    void UpdatePosition()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            TileGenWalkerObject FoundWalker = Walkers[i];
            FoundWalker.Position += FoundWalker.Direction;
            FoundWalker.Position.x = Mathf.Clamp(FoundWalker.Position.x, 1, gridHandler.GetLength(0) - 2);
            FoundWalker.Position.y = Mathf.Clamp(FoundWalker.Position.y, 1, gridHandler.GetLength(1) - 2);
            Walkers[i] = FoundWalker;
        }
    }

}