using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
this is probably the biggest code file in the project. it is mainly this video's code, and our game's logic on top
---> https://www.youtube.com/watch?v=6B7yOnqpK_Y / https://github.com/GarnetKane99/RandomWalkerAlgo_YT
1. generate an empty MapWidth x MapHeight grid, datatype Grid. fill it with Grid.EMPTY
2. spawn some "walkers" in the middle, and let them walk from the middle, and wherever they walk, they turn
Grid.EMPTY into Grid.ONE_ONE. this represents a ROOM SPACE for now, not neccesarily a 1x1 room.
3. randomly pick a point in this until you hit a Grid.ONE_ONE, and turn it into Grid.CONTROL_ROOM. cuz only one spawns
4. run through every tile in the grid. if it's ONE_ONE check it's neighbors to see if you can spawn a 2x2 room. roll the 
chance to get a 2x2 room, and if successful, change the tiles to Grid.TWO_TWO and run DrawTwoByTwo().
5. similar logic for 2x1 vertical, and 2x1 horizontal rooms, and DrawTwoByOneVertical()/Horizontal()
6. run through the entire grid again. if we land on a ONE_ONE room, draw it.
(note: drawing means spawning all the assets in it so they're in the actual unity scene).
(note: in the running of the Draw commands, we also spawn navmeshes on the whole map corresponding to each room)
7. bake the navmesh so we have one big area where AI can walk on
8. run PostMapGenFunction. this will generate doors so they do not interfere with A.I. pathfinding and also run
PostmapGenScript for anything else.

-alin, and optimized by william fridh
*/

public class RandomMapHandler : MonoBehaviour
{
    [Tooltip("list of all the room floors that are 1x1")]
    public List<GameObject> One_OneFloors = new List<GameObject>();
    [Tooltip("list of all the room walls that are 1x1")]
    public List<GameObject> One_OneWalls = new List<GameObject>();
    [Tooltip("list of all the room doorways that correspond to 1x1")]
    public List<GameObject> One_OneDoorways = new List<GameObject>();
    [Tooltip("list of all the room floors that are 2x1")]
    public List<GameObject> Two_OneFloors = new List<GameObject>();
    [Tooltip("list of all the room walls that are 2x1")]
    public List<GameObject> Two_OneWalls = new List<GameObject>();
    [Tooltip("list of all the room doorways that correspond to 2x1")]
    public List<GameObject> Two_OneDoorways = new List<GameObject>();
    [Tooltip("list of all the room floors that are 2x2")]
    public List<GameObject> Two_TwoFloors = new List<GameObject>();
    [Tooltip("list of all the room walls that are 2x2")]
    public List<GameObject> Two_TwoWalls = new List<GameObject>();
    [Tooltip("list of all the room doorways that correspond to 2x2")]
    public List<GameObject> Two_TwoDoorways = new List<GameObject>();
    [Tooltip("this should keep the prefab for the control room floor only, will use prefab nr0 for its walls/doorways")]
    public GameObject ControlRoomFloor;
    [Tooltip("list of all the room walls that are 2x2")]
    public GameObject ControlWall;
    [Tooltip("list of all the room doorways that correspond to 2x2")]
    public GameObject ControlDoorway;
    [Tooltip("list of all navmeshes in ORDER: 1x1, 2x1, 2x2, doorway")]
    public List<GameObject> Navmeshes = new List<GameObject>();
    [Tooltip("list of all door prefabs that will generate in doorways")]
    public List<GameObject> Doors = new List<GameObject>();
    [Tooltip("prefab with the escape door specifically")]
    public GameObject escapeDoor;
    [Tooltip("prefab with the lore note")]
    public GameObject LoreNote;
    [Tooltip("amount of lore notes to spawn")]
    public int LoreNoteAmount;
    private int searchOffset = 0, escapeX, escapeZ;
    private string direction;
    private Boolean found = false;

    //this is used to store all doors generated and spawn them AFTER the navmesh si generated, so monsters can path through doors
    //xPosition, zPosition, yRotation
    LinkedList<(float, float, float)> DoorList = new LinkedList<(float, float, float)>();

    

    public enum Grid
    {
        CONTROL_ROOM,
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
    public int RoomSize = 5; // THIS IS HOW BIG ONE "SQUARE" ROOM OF 1x1 IS IN UNITY UNITS!
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
    [Tooltip("(0 -> 1), this decides the percentage chance that doorways will have a door")]
    public float DoorChance = 0.50f;
    [Tooltip("this is the gameobject which the entire map will be parented to")]
    public GameObject RandomMapParent;
    [Tooltip("this is the gameobject that navmeshes will be parented to (VERY IMPORTANT)")]
    public GameObject NavmeshParent;
    [Tooltip("this hosts a script that runs right when mapgen ends for convenience")]
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
        found = false;
        while(!found){
            //down to up on left side
            for(int z = searchOffset; z<MapHeight - searchOffset; z++){
                if(gridHandler[searchOffset, z] != RandomMapHandler.Grid.EMPTY &&
                   gridHandler[searchOffset, z] != RandomMapHandler.Grid.ONE_ONE && 
                   gridHandler[searchOffset, z] != RandomMapHandler.Grid.CONTROL_ROOM &&
                   !found){
                    escapeX = searchOffset;
                    escapeZ = z;
                    found = true;
                    direction = "left";
                }
            }
            //left to right on top side
            for(int x = searchOffset; x<MapWidth - searchOffset; x++){
                if(gridHandler[x, MapHeight - searchOffset - 1] != RandomMapHandler.Grid.EMPTY && 
                   gridHandler[x, MapHeight - searchOffset - 1] != RandomMapHandler.Grid.ONE_ONE && 
                   gridHandler[x, MapHeight - searchOffset - 1] != RandomMapHandler.Grid.CONTROL_ROOM &&
                   !found){
                    escapeX = x;
                    escapeZ = MapHeight - searchOffset - 1;
                    found = true;
                    direction = "up";
                }
            }
            //down to up on right side
            for(int z = searchOffset; z<MapHeight - searchOffset; z++){
                if(gridHandler[MapWidth - searchOffset - 1, z] != RandomMapHandler.Grid.EMPTY && 
                   gridHandler[MapWidth - searchOffset - 1, z] != RandomMapHandler.Grid.ONE_ONE &&
                   gridHandler[MapWidth - searchOffset - 1, z] != RandomMapHandler.Grid.CONTROL_ROOM && 
                   !found){
                    escapeX = MapWidth - searchOffset - 1;
                    escapeZ = z;
                    found = true;
                    direction = "right";
                }
            }
            //left to right on bottom side
            for(int x = searchOffset; x<MapWidth - searchOffset; x++){
                if(gridHandler[x, searchOffset] != RandomMapHandler.Grid.EMPTY && 
                   gridHandler[x, searchOffset] != RandomMapHandler.Grid.ONE_ONE &&
                   gridHandler[x, searchOffset] != RandomMapHandler.Grid.CONTROL_ROOM && 
                   !found){
                    escapeX = x;
                    escapeZ = searchOffset;
                    found = true;
                    direction = "down";
                }
            }
            searchOffset++;    
        }
        //Debug.Log("the door coords: (" + escapeX + ", " + escapeZ + ")");
        if(direction == "left"){
            Instantiate(escapeDoor, new Vector3(escapeX * RoomSize - RoomSize/2 + 0.31f, 0, escapeZ * RoomSize),
            Quaternion.Euler(0, 90, 0)); 
        }
        else if(direction == "up"){
            Instantiate(escapeDoor, new Vector3(escapeX * RoomSize, 0, escapeZ * RoomSize + RoomSize/2 - 0.31f),
            Quaternion.Euler(0, 180, 0));
        }
        else if(direction == "right"){
            Instantiate(escapeDoor, new Vector3(escapeX * RoomSize + RoomSize/2 - 0.31f, 0, escapeZ * RoomSize),
            Quaternion.Euler(0, -90, 0));
        }
        else if(direction == "down"){
            Instantiate(escapeDoor, new Vector3(escapeX * RoomSize, 0, escapeZ * RoomSize - RoomSize/2 + 0.31f),
            Quaternion.identity);
        }
        navScript.GenerateMesh(); //THIS IS THE PART WHERE THE NAVMESH IS BAKED AT
        while (DoorList.First != null)
        {
            float x, z, rot;
            (x,z,rot) = DoorList.First.Value;
            Instantiate
            (
                Doors[UnityEngine.Random.Range(0,Doors.Count)], //THIS IS TEMPORARY BECAUSE I DON'T THINK WE'LL HAVE DOORS CORRESPOND TO ROOMTYPES
                new Vector3(x, 0 , z),
                Quaternion.Euler(0, rot, 0)
            ).transform.SetParent(NavmeshParent.transform, false);
            DoorList.RemoveFirst();
        }
        player.transform.position = RandomMapParent.transform.position + new Vector3((gridHandler.GetLength(0)/2)*RoomSize,5,(gridHandler.GetLength(1)/2)*RoomSize);
    }

    /* initialize navmesh
    this smacks a navmesh down at the same time as a 2x2/2x1/1x1 floor you need
    so that they are all at the same height and are parented to the same thing
    once they are parented to the same thing you can hit "bake" on that big thing and it'll
    make the navmesh we need for the monster to move around

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
    void InitializePrefab(string type, int x, int y, bool generateDoorWay, int prefabIndex, float xPosition, float zPosition, float yRotation)
    {

        // Select prefab.
        GameObject prefab;
        if (generateDoorWay)
            {
                switch (type)
                {
                    case "control":
                        prefab = ControlDoorway;
                        break;
                    case "2x1":
                        prefab = Two_OneDoorways[prefabIndex];
                        break;
                    case "2x2":
                        prefab = Two_TwoDoorways[prefabIndex];
                        break;
                    default: //case 1x1
                        prefab = One_OneDoorways[prefabIndex];
                        break;

                
                }
                
                //add navmesh
                Instantiate(
                Navmeshes[3],
                new Vector3(xPosition, 1 , zPosition),
                Quaternion.Euler(0, yRotation, 0)
                ).transform.SetParent(NavmeshParent.transform, false);

                //add doorway
                if (UnityEngine.Random.value < DoorChance && (yRotation != 180f) && (yRotation != 270f)) //this makes it so they only generate N and E to prevent overlapping doors
                {
                    DoorList.AddFirst((xPosition, zPosition, yRotation));
                }
            }
        else
            switch (type)
                {
                    case "control":
                        prefab = ControlWall;
                        break;
                    case "2x1":
                        prefab = Two_OneWalls[prefabIndex];
                        break;
                    case "2x2":
                        prefab = Two_TwoWalls[prefabIndex];
                        break;
                    default: //case 1x1
                        prefab = One_OneWalls[prefabIndex];
                        break;
                
                }
        
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
        InitializePrefab("1x1",x, y, n, prefabIndex, (x)*RoomSize, (y+0.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab("1x1",x, y, e, prefabIndex, (x+0.50f - WallGapSize)*RoomSize, (y)*RoomSize, 90f);
        InitializePrefab("1x1",x, y, s, prefabIndex, (x)*RoomSize, (y-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab("1x1",x, y, w, prefabIndex, (x-0.50f + WallGapSize)*RoomSize, (y)*RoomSize, 270f);
    }
    void DrawTwoByTwo(int x, int y, bool a, bool b, bool c, bool d, bool e, bool f, bool g, bool h) //THIS DRAWS A 2x2 FLOOR
    {
        int prefabIndex = UnityEngine.Random.Range(0,Two_TwoFloors.Count); //pick variant of 2x2
        //make floor
        Instantiate(Two_TwoFloors[prefabIndex], new Vector3((x+0.5f)*RoomSize, 0, (y+0.5f)*RoomSize), Quaternion.Euler(0,0,0)).transform.SetParent(RandomMapParent.transform, false);
        InitializeNavmesh(x, y, "2x2");
        InitializePrefab("2x2",x, y, a, prefabIndex, (x)*RoomSize, (y+1.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab("2x2",x, y, b, prefabIndex, (x+1f)*RoomSize, (y+1.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab("2x2",x, y, c, prefabIndex, (x+1.50f - WallGapSize)*RoomSize, (y+1)*RoomSize, 90f);
        InitializePrefab("2x2",x, y, d, prefabIndex, (x+1.50f - WallGapSize)*RoomSize, (y)*RoomSize, 90f);
        InitializePrefab("2x2",x, y, e, prefabIndex, (x+1f)*RoomSize, (y-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab("2x2",x, y, f, prefabIndex, (x)*RoomSize, (y-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab("2x2",x, y, g, prefabIndex, (x-0.50f + WallGapSize)*RoomSize, (y)*RoomSize, 270f);
        InitializePrefab("2x2",x, y, h, prefabIndex, (x-0.50f + WallGapSize)*RoomSize, (y+1f)*RoomSize, 270f);
    }
    void DrawTwoByOneHorizontal(int x, int y, bool a, bool b, bool c, bool d, bool e, bool f) //THIS DRAWS A 2x1 FLOOR
    {
        int prefabIndex = UnityEngine.Random.Range(0,Two_OneFloors.Count); //pick variant of 2x1
        //make floor
        Instantiate(Two_OneFloors[prefabIndex], new Vector3((x+0.5f)*RoomSize, 0, (y)*RoomSize), Quaternion.Euler(0,0,0)).transform.SetParent(RandomMapParent.transform, false);
        InitializeNavmesh(x, y, "2x1Horizontal");
        InitializePrefab("2x1",x, y, a, prefabIndex, (x)*RoomSize, (y+0.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab("2x1",x, y, b, prefabIndex, (x+1f)*RoomSize, (y+0.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab("2x1",x, y, c, prefabIndex, (x+1.50f - WallGapSize)*RoomSize, (y)*RoomSize, 90f);
        InitializePrefab("2x1",x, y, d, prefabIndex, (x+1f)*RoomSize, (y-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab("2x1",x, y, e, prefabIndex, (x)*RoomSize, (y-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab("2x1",x, y, f, prefabIndex, (x-0.50f + WallGapSize)*RoomSize, (y)*RoomSize, 270f);   
    }
    void DrawTwoByOneVertical(int x, int y, bool a, bool b, bool c, bool d, bool e, bool f) //THIS DRAWS A 2x1 FLOOR
    {
        int prefabIndex = UnityEngine.Random.Range(0,Two_OneFloors.Count); //pick variant of 2x1
        //make floor
        Instantiate(Two_OneFloors[prefabIndex], new Vector3((x)*RoomSize, 0, (y+0.5f)*RoomSize), Quaternion.Euler(0,90f,0)).transform.SetParent(RandomMapParent.transform, false);
        InitializeNavmesh(x, y, "2x1Vertical");
        InitializePrefab("2x1",x, y, a, prefabIndex, (x)*RoomSize, (y+1.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab("2x1",x, y, b, prefabIndex, (x+0.50f - WallGapSize)*RoomSize, (y+1f)*RoomSize, 90f);
        InitializePrefab("2x1",x, y, c, prefabIndex, (x+0.50f - WallGapSize)*RoomSize, (y)*RoomSize, 90f);
        InitializePrefab("2x1",x, y, d, prefabIndex, (x)*RoomSize, (y-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab("2x1",x, y, e, prefabIndex, (x-0.50f + WallGapSize)*RoomSize, (y)*RoomSize, 270f);
        InitializePrefab("2x1",x, y, f, prefabIndex, (x-0.50f + WallGapSize)*RoomSize, (y+1f)*RoomSize, 270f); 
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

        //CONTROL ROOM HANDLING, MAKING SURE AT LEAST ONE SPAWNS
        int controlroomX = UnityEngine.Random.Range(0,MapWidth); 
        int controlroomY = UnityEngine.Random.Range(0,MapHeight);
        while ((gridHandler[controlroomX,controlroomY] != Grid.ONE_ONE) || ((controlroomX == MapWidth/2) && (controlroomY == MapHeight/2))) //redoing this until we randomly pick a spot thats valid
        {
            controlroomX = UnityEngine.Random.Range(0,MapWidth);
            controlroomY = UnityEngine.Random.Range(0,MapHeight);
        }
        gridHandler[controlroomX,controlroomY] = Grid.CONTROL_ROOM;
        Instantiate(ControlRoomFloor, new Vector3((controlroomX)*RoomSize, 0, (controlroomY)*RoomSize), Quaternion.Euler(0,0,0)).transform.SetParent(RandomMapParent.transform, false);
        bool north = false, east = false, south = false, west = false;
        try { north = gridHandler[controlroomX,controlroomY+1] != Grid.EMPTY;}
        catch (Exception) {}
        try { east = gridHandler[controlroomX+1,controlroomY] != Grid.EMPTY;}
        catch (Exception) {}
        try { south = gridHandler[controlroomX,controlroomY-1] != Grid.EMPTY;}
        catch (Exception) {}
        try { west = gridHandler[controlroomX-1,controlroomY] != Grid.EMPTY;}
        catch (Exception) {}
        InitializePrefab("control",controlroomX, controlroomY, north, 0, (controlroomX)*RoomSize, (controlroomY+0.50f - WallGapSize)*RoomSize, 0);
        InitializePrefab("control",controlroomX, controlroomY, east, 0, (controlroomX+0.50f - WallGapSize)*RoomSize, (controlroomY)*RoomSize, 90f);
        InitializePrefab("control",controlroomX, controlroomY, south, 0, (controlroomX)*RoomSize, (controlroomY-0.50f + WallGapSize)*RoomSize, 180f);
        InitializePrefab("control",controlroomX, controlroomY, west, 0, (controlroomX-0.50f + WallGapSize)*RoomSize, (controlroomY)*RoomSize, 270f);
        

        //LORE PAPER HANDLING, MAKING SURE THAT ALL OF THE SUBMITTED ONES SPAWN AT RANDOM POINTS IN THE MAP
        for (int i = 0; i < LoreNoteAmount ; i++)
        {
            int paperX = UnityEngine.Random.Range(0,MapWidth); 
            int paperY = UnityEngine.Random.Range(0,MapHeight);
            while ((gridHandler[paperX,paperY]) != Grid.ONE_ONE) //redoing this until we randomly pick a spot thats valid
            {
                paperX = UnityEngine.Random.Range(0,MapWidth);
                paperY = UnityEngine.Random.Range(0,MapHeight);
            }

            Instantiate(LoreNote, new Vector3((paperX)*RoomSize, 0.2f, (paperY)*RoomSize), Quaternion.Euler(0,0,0));
        }


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
                    north = false;
                    east = false;
                    south = false;
                    west = false;  //variables to hand to
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