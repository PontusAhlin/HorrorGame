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

public class RandomMapHandler : MonoBehaviour
{
    [Tooltip("list of all the room floors that are 1x1")]
    public List<GameObject> One_OneFloors = new List<GameObject>();
    [Tooltip("list of all the room floors that are 2x1")]
    public List<GameObject> Two_OneFloors = new List<GameObject>();
    [Tooltip("list of all the room floors that are 2x2")]
    public List<GameObject> Two_TwoFloors = new List<GameObject>();
    [Tooltip("list of all the room walls that correspond to 2x2 and 2x1")]
    public List<GameObject> Walls = new List<GameObject>();
    [Tooltip("list of all the room doorways that correspond to 2x2 and 2x1")]
    public List<GameObject> Doorways = new List<GameObject>();
    [Tooltip("list of all navmeshes in ORDER: 1x1, 2x1, 2x2, doorway")]
    public List<GameObject> Navmeshes = new List<GameObject>();
    

    public enum Grid
    {
        TWO_TWO,
        TWO_ONE,
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
    [Tooltip("this decides the distance between the complete edge of the cell and where the wall spawns")]
    public float WallGapSize = 0.01f;
    [Tooltip("more walkers - more uhhh...agents walking around generating the map. imagine you have ants on a piece of paper that u dunked in ink. wherever the ant walks, the map generates. this value sets the amount of ants u drop down")]
    public int MaximumWalkers = 10;
    [Tooltip("not relevant, just shows in editor how many tiles have been made, for debugging")]
    public int TileCount = default;
    [Tooltip("(0 -> 1), percentage of the map's total cells to be filled until it's done")]
    public float FillPercentage = 0.4f;
    [Tooltip("(0 -> 1), decides how chaotic the random gen is. lower = straighter rooms")]
    public float Randomness = 0.50f;
    [Tooltip("float value, but this should be like ZERO because all it does is delay the thing and lag!!")]
    public float WaitTime = 0.05f;
    [Tooltip("(0 -> 1), this decides the percentage chance that 2x2 rooms will be generated when possible")]
    public float TwoByTwoChance = 0.50f;
    [Tooltip("(0 -> 1), this decides the percentage chance that 1x2 rooms will be generated when possible")]
    public float TwoByOneChance = 0.50f;
    [Tooltip("this is the gameobject which the entire map will be parented to")]
    public GameObject RandomMapParent;
    [Tooltip("this is the gameobject that navmeshes will be parented to (VERY IMPORTANT)")]
    public GameObject NavmeshParent;
    [Tooltip("this hosts a script that runs right when mapgen ends for convenience")]
    public PostMapgenScript postMapgenScript;
    [Tooltip("this hosts the script for navmesh generation because we have a call to that")]
    public NavmeshGenerator navScript;
    [Tooltip("this hosts the player object so that we can teleport it on a map tile when its done generatin")]
    public GameObject player;
    void Start()
    {
        if (FillPercentage > 0.5f)
        {
            Debug.LogWarning("FILL PERCENTAGE TOO HIGH, CLAMPING (THIS CAUSES A CRASH IF IT'S OVER 0.5f IDK WHY -ALIN)");
            FillPercentage = 0.5f;
        }
        InitializeGrid();
    }
    //THIS FUNCTION RUNS AFTER MAPGEN IS DONE FOR HANDLING STUFF LIKE PLAYER & MONSTER PLACEMENT
    void PostMapgenFunction()
    {
        Debug.Log("PostMapgenFunction ran");
        navScript.GenerateMesh();
        player.transform.position = RandomMapParent.transform.position + new Vector3((gridHandler.GetLength(0)/2)*RoomSize,5,(gridHandler.GetLength(1)/2)*RoomSize);
        postMapgenScript.Main();
    }

    /* initialize navmesh
    this smacks a navmesh down at the same time as a big/small/whateverthefuck floor you need
    so that they are all at the same height and are parented to the same thing
    once theyr e parented to the same thing you can hit "bake" on that big thing and it'll
    make the navmesh we need for the mosnter to move around

    -alin
    */
    void InitializeNavmesh(int x, int y, string roomType)
    {
        switch (roomType)
        {
            case "1x1":
            {
                Instantiate(Navmeshes[0], new Vector3((x)*RoomSize, 1, (y)*RoomSize), Quaternion.Euler(0,0,0)).transform.SetParent(NavmeshParent.transform, false);
                break;
            }
            case "2x1Horizontal":
            {
                Instantiate(Navmeshes[1], new Vector3((x+0.5f)*RoomSize, 1, (y)*RoomSize), Quaternion.Euler(0,0,0)).transform.SetParent(NavmeshParent.transform, false);
                break;
            }
            case "2x1Vertical":
            {
                Instantiate(Navmeshes[1], new Vector3((x)*RoomSize, 1, (y+0.5f)*RoomSize), Quaternion.Euler(0,90f,0)).transform.SetParent(NavmeshParent.transform, false);
                break;
            }
            case "2x2":
            {
                Instantiate(Navmeshes[2], new Vector3((x+0.5f)*RoomSize, 1, (y+0.5f)*RoomSize), Quaternion.Euler(0,0,0)).transform.SetParent(NavmeshParent.transform, false);
                break;
            }
        }
    }

    /**
      * Initilize Big Prefab. ALIN NOTE: this doesnt need x and y but i will fix this l8r
      *
      * This is a helper function used for generating prefabs. It was created to shorten down the
      * code and make it more reable due to similar code being used multiple times.
      * It loads prefabs from BigWalls and BigDoorways lists and instantiates them at the
      * given position.
      *
      * Note that no y-position is needed as the y-position is always 0.
      *
      * Author(s): William Fridh
      */
    void InitializePrefab(int x, int y, bool generateDoorWay, int prefabIndex, float xPosition, float zPosition, float yRotation)
    {

        // Select prefab.
        GameObject prefab;
        if (generateDoorWay)
            {
                prefab = Doorways[prefabIndex];
                //add navmesh
                Instantiate(
                Navmeshes[3],
                new Vector3(xPosition, 1 , zPosition),
                Quaternion.Euler(0, yRotation, 0)
                ).transform.SetParent(NavmeshParent.transform, false);
            }
        else
            prefab = Walls[prefabIndex];
        
        // Add prefab.
        Instantiate(
            prefab,
            new Vector3(xPosition, 0, zPosition),
            Quaternion.Euler(0, yRotation, 0)
        ).transform.SetParent(RandomMapParent.transform, false);

    }

    void DrawOneByOne(int x, int y, bool n, bool e, bool s, bool w)
    {
        int prefabIndex = UnityEngine.Random.Range(0,One_OneFloors.Count); //pick variant of 2x2
        //make floor
        Instantiate(One_OneFloors[prefabIndex], new Vector3((x)*RoomSize, 0, (y)*RoomSize), Quaternion.Euler(0,0,0)).transform.SetParent(RandomMapParent.transform, false);
        InitializeNavmesh(x, y, "1x1");
        InitializePrefab(x, y, n, prefabIndex, (x)*RoomSize, (y+0.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab(x, y, e, prefabIndex, (x+0.50f - WallGapSize)*RoomSize, (y)*RoomSize, 90f);
        InitializePrefab(x, y, s, prefabIndex, (x)*RoomSize, (y-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab(x, y, w, prefabIndex, (x-0.50f + WallGapSize)*RoomSize, (y)*RoomSize, 270f);
    }
    void DrawTwoByTwo(int x, int y, bool a, bool b, bool c, bool d, bool e, bool f, bool g, bool h) //THIS DRAWS A 2x2 FLOOR
    {
        int prefabIndex = UnityEngine.Random.Range(0,Two_TwoFloors.Count); //pick variant of 2x2
        //make floor
        Instantiate(Two_TwoFloors[prefabIndex], new Vector3((x+0.5f)*RoomSize, 0, (y+0.5f)*RoomSize), Quaternion.Euler(0,0,0)).transform.SetParent(RandomMapParent.transform, false);
        InitializeNavmesh(x, y, "2x2");
        InitializePrefab(x, y, a, prefabIndex, (x)*RoomSize, (y+1.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab(x, y, b, prefabIndex, (x+1f)*RoomSize, (y+1.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab(x, y, c, prefabIndex, (x+1.50f - WallGapSize)*RoomSize, (y+1)*RoomSize, 90f);
        InitializePrefab(x, y, d, prefabIndex, (x+1.50f - WallGapSize)*RoomSize, (y)*RoomSize, 90f);
        InitializePrefab(x, y, e, prefabIndex, (x+1f)*RoomSize, (y-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab(x, y, f, prefabIndex, (x)*RoomSize, (y-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab(x, y, g, prefabIndex, (x-0.50f + WallGapSize)*RoomSize, (y)*RoomSize, 270f);
        InitializePrefab(x, y, h, prefabIndex, (x-0.50f + WallGapSize)*RoomSize, (y+1f)*RoomSize, 270f);
    }
    void DrawTwoByOneHorizontal(int x, int y, bool a, bool b, bool c, bool d, bool e, bool f) //THIS DRAWS A 2x1 FLOOR
    {
        int prefabIndex = UnityEngine.Random.Range(0,Two_OneFloors.Count); //pick variant of 2x1
        //make floor
        Instantiate(Two_OneFloors[prefabIndex], new Vector3((x+0.5f)*RoomSize, 0, (y)*RoomSize), Quaternion.Euler(0,0,0)).transform.SetParent(RandomMapParent.transform, false);
        InitializeNavmesh(x, y, "2x1Horizontal");
        InitializePrefab(x, y, a, prefabIndex, (x)*RoomSize, (y+0.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab(x, y, b, prefabIndex, (x+1f)*RoomSize, (y+0.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab(x, y, c, prefabIndex, (x+1.50f - WallGapSize)*RoomSize, (y)*RoomSize, 90f);
        InitializePrefab(x, y, d, prefabIndex, (x+1f)*RoomSize, (y-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab(x, y, e, prefabIndex, (x)*RoomSize, (y-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab(x, y, f, prefabIndex, (x-0.50f + WallGapSize)*RoomSize, (y)*RoomSize, 270f);   
    }
    void DrawTwoByOneVertical(int x, int y, bool a, bool b, bool c, bool d, bool e, bool f) //THIS DRAWS A 2x1 FLOOR
    {
        int prefabIndex = UnityEngine.Random.Range(0,Two_OneFloors.Count); //pick variant of 2x1
        //make floor
        Instantiate(Two_OneFloors[prefabIndex], new Vector3((x)*RoomSize, 0, (y+0.5f)*RoomSize), Quaternion.Euler(0,90f,0)).transform.SetParent(RandomMapParent.transform, false);
        InitializeNavmesh(x, y, "2x1Vertical");
        InitializePrefab(x, y, a, prefabIndex, (x)*RoomSize, (y+1.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab(x, y, b, prefabIndex, (x+0.50f - WallGapSize)*RoomSize, (y+1f)*RoomSize, 90f);
        InitializePrefab(x, y, c, prefabIndex, (x+0.50f - WallGapSize)*RoomSize, (y)*RoomSize, 90f);
        InitializePrefab(x, y, d, prefabIndex, (x)*RoomSize, (y-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab(x, y, e, prefabIndex, (x-0.50f + WallGapSize)*RoomSize, (y)*RoomSize, 270f);
        InitializePrefab(x, y, f, prefabIndex, (x-0.50f + WallGapSize)*RoomSize, (y+1f)*RoomSize, 270f); 
    }
    IEnumerator CreateFloors()
    {
        while ((float)TileCount / (float)gridHandler.Length < FillPercentage)
        {
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
        for (int x = 0; x < MapWidth-1; x++) //APPLYING TWO_TWO ROOMS
            for (int y = 0; y < MapHeight-1; y++)
            {
                bool gotSpace = false;
                try
                {
                    gotSpace = gridHandler[x,y] == Grid.ONE_ONE && 
                    gridHandler[x,y+1] == Grid.ONE_ONE &&
                    gridHandler[x+1,y] == Grid.ONE_ONE &&
                    gridHandler[x+1,y+1] == Grid.ONE_ONE;
                }
                catch (Exception) {gotSpace = false;}
                if (gotSpace && (UnityEngine.Random.value < TwoByTwoChance)) //for a 2x2 grid block
                    { //place a TWO_TWO
                        gridHandler[x,y] = Grid.TWO_TWO; 
                        gridHandler[x,y+1] = Grid.TWO_TWO;
                        gridHandler[x+1,y] = Grid.TWO_TWO;
                        gridHandler[x+1,y+1] = Grid.TWO_TWO;
                        bool a=false,b=false,c=false,d=false,e=false,f=false,g=false,h = false;
                        try { a = gridHandler[x,y+2] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { b = gridHandler[x+1,y+2] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { c = gridHandler[x+2,y+1] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { d = gridHandler[x+2,y] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { e = gridHandler[x+1,y-1] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { f = gridHandler[x,y-1] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { g = gridHandler[x-1,y] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { h = gridHandler[x-1,y+1] != Grid.EMPTY;}
                        catch (Exception) {}
                        DrawTwoByTwo(x,y,a,b,c,d,e,f,g,h);
                    }
            }

        for (int x = 0; x < MapWidth-1; x++) //APPLYING TWO_ONE ROOMS, VERTICAL
            for (int y = 0; y < MapHeight-1; y++)
            {
                bool gotSpace = false;
                try
                {
                    gotSpace = gridHandler[x,y] == Grid.ONE_ONE && 
                    gridHandler[x,y+1] == Grid.ONE_ONE;
                }
                catch (Exception) {gotSpace = false;}
                if (gotSpace && (UnityEngine.Random.value < TwoByOneChance)) //for a 2x2 grid block
                    { //place a TWO_TWO
                        gridHandler[x,y] = Grid.TWO_ONE; 
                        gridHandler[x,y+1] = Grid.TWO_ONE;
                        bool a=false,b=false,c=false,d=false,e=false,f=false;
                        try { a = gridHandler[x,y+2] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { b = gridHandler[x+1,y+1] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { c = gridHandler[x+1,y] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { d = gridHandler[x,y-1] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { e = gridHandler[x-1,y] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { f = gridHandler[x-1,y+1] != Grid.EMPTY;}
                        catch (Exception) {}
                        DrawTwoByOneVertical(x,y,a,b,c,d,e,f);
                    }
            }

        for (int x = 0; x < MapWidth-1; x++) //APPLYING TWO_ONE ROOMS, HORIZONTAL
            for (int y = 0; y < MapHeight-1; y++)
            {
                bool gotSpace = false;
                try
                {
                    gotSpace = gridHandler[x,y] == Grid.ONE_ONE && 
                    gridHandler[x+1,y] == Grid.ONE_ONE;
                }
                catch (Exception) {gotSpace = false;}
                if (gotSpace && (UnityEngine.Random.value < TwoByOneChance)) //for a 2x2 grid block
                    { //place a TWO_TWO
                        gridHandler[x,y] = Grid.TWO_ONE; 
                        gridHandler[x+1,y] = Grid.TWO_ONE;
                        bool a=false,b=false,c=false,d=false,e=false,f=false;
                        try { a = gridHandler[x,y+1] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { b = gridHandler[x+1,y+1] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { c = gridHandler[x+2,y] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { d = gridHandler[x+1,y-1] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { e = gridHandler[x,y-1] != Grid.EMPTY;}
                        catch (Exception) {}
                        try { f = gridHandler[x-1,y] != Grid.EMPTY;}
                        catch (Exception) {}
                        DrawTwoByOneHorizontal(x,y,a,b,c,d,e,f);
                    }
            }


        for (int x = 0; x < MapWidth; x++) //APPLYING 1X1 ROOMS
            for (int y = 0; y < MapHeight; y++)
            {
                if (gridHandler[x, y] == Grid.ONE_ONE)
                {
                    bool north = false, east = false, south = false, west = false;  //variables to hand to
                                                                                    //DrawRoom to select the right prefab
                                                                                    //with n/e/s/w entrances
                    try { north = gridHandler[x,y+1] != Grid.EMPTY;}
                    catch (Exception) {}
                    try { east = gridHandler[x+1,y] != Grid.EMPTY;}
                    catch (Exception) {}
                    try { south = gridHandler[x,y-1] != Grid.EMPTY;}
                    catch (Exception) {}
                    try { west = gridHandler[x-1,y] != Grid.EMPTY;}
                    catch (Exception) {}
                    DrawOneByOne(x,y,north,east,south,west);
                }
                
            }
        PostMapgenFunction();
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
}