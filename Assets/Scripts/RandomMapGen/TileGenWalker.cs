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
    public List<GameObject> prefabList = new List<GameObject>();
    public enum Grid
    {
        FLOOR,
        EMPTY
    }

    //Variables
    public Grid[,] gridHandler;
    public List<TileGenWalkerObject> Walkers; 
    public int MapWidth = 30; //map size 
    public int MapHeight = 30;
    public int RoomSize = 5; // THIS IS HOW BIG ONE "SQUARE" ROOM OF 1x1 IS IN UNITY UNITS!!

    public int MaximumWalkers = 10; //amount of Walkers generating the map. think of these as 
    //ants walking around from (middlex, middley) outwards, and everywhere they step
    // is a room, and everywhere they don't is NOT a room. more walkers = blobbier rooms
    public int TileCount = default; //dont touch this, this is just so u can see it in editor and doesn't
    //affect how the generation runs
    public float FillPercentage = 0.4f; //now THIS is what decides how much of the map should be filled, 
    // where 100% is all the rooms of MapWidth*MapHeight
    public float WaitTime = 0.05f; //manual delay in generating the map, for debugging purposes.
    //THIS SHOULDBE AS LOW AS POSSIBLE!!!!

    void Start()
    {
        InitializeGrid();
    }
    //this is the function that is called at coordinates X & Y 
    void DrawRoom(int x, int y, bool north, bool east, bool south, bool west)
    {
        Debug.Log("drawn room at (" + x + "," + y + ")");
        int prefabIndex = UnityEngine.Random.Range(0,prefabList.Count);
        Instantiate(prefabList[prefabIndex], new Vector3(x*RoomSize, 0, y*RoomSize), Quaternion.identity);
    }


    /*
    THIS FUNCTION JUST DRAWS CUBES IN THE EDITOR. 
    THIS IS FOR TESTING PURPOSES. UNCOMMENT TO SEE IT WHEN U
    CLICK PLAY AND THEN GO TO SCENE

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        for (int i = 0; i < MapWidth ; i++)
            for (int j = 0; j < MapHeight ; j++)
            {
                if (gridHandler[i, j] == Grid.FLOOR)
                    {   
                        Vector3 pos = new Vector3(i, 0, j);
                        Gizmos.DrawCube(pos, Vector3.one);
                    }
            }

    }
    */

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

        TileGenWalkerObject curWalker = new TileGenWalkerObject(new Vector2(TileCenter.x, TileCenter.y), GetDirection(), 0.5f);
        gridHandler[TileCenter.x, TileCenter.y] = Grid.FLOOR;
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
            bool hasCreatedFloor = false;
            foreach (TileGenWalkerObject curWalker in Walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);

                if (gridHandler[curPos.x, curPos.y] != Grid.FLOOR)
                {
                    TileCount++;
                    gridHandler[curPos.x, curPos.y] = Grid.FLOOR;
                    hasCreatedFloor = true;
                }
            }

            //Walker Methods
            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();

            if (hasCreatedFloor)
            {
                yield return new WaitForSeconds(WaitTime);
            }
        }

        /*
          THIS IS WHERE YOU SHOULD WRITE CODE THAT CARES ABOUT THE MAP
          LIKE
          THIS IS THE POINT WHERE THE MAP IS FULL OF Grid.EMPTY or GRID.FLOOR
        */

        for (int x = 0; x < MapWidth; x++) //HANDLING IF THERE ARE ANY ADJACENT GRID STUFFS
            for (int y = 0; y < MapHeight; y++)
            {
                if (gridHandler[x, y] == Grid.FLOOR)
                {
                    bool north = false, east = false, south = false, west = false;  //variables to hand to
                                                                                    //DrawRoom to select the right prefab
                                                                                    //with n/e/s/w entrances
                    // CHECKING NORTH
                    try
                    {
                        if (gridHandler[x,y+1] == Grid.FLOOR)
                            north = true;
                    }
                    catch (Exception) {}
                    // CHECKING EAST
                    try
                    {
                        if (gridHandler[x+1,y] == Grid.FLOOR)
                            east = true;
                    }
                    catch (Exception) {}
                    // CHECKING SOUTH
                    try
                    {
                        if (gridHandler[x,y-1] == Grid.FLOOR)
                            south = true;
                    }
                    catch (Exception) {}
                    // CHECKING WEST
                    try
                    {
                        if (gridHandler[x-1,y] == Grid.FLOOR)
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

                TileGenWalkerObject newWalker = new TileGenWalkerObject(newPosition, newDirection, 0.5f);
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