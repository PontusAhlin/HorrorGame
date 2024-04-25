using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*BY: ALIN
The purpose of this script is to be ran RIGHT AFTER the map is done generating, for stuff like
handling spawnign the player, the monster, etcetera. you can attach and call
other scripts from here too.
*/

public class PostMapgenScript : MonoBehaviour{
    public GameObject escapeDoor;
    public GameObject RandomMapParent;
    public RandomMapHandler randscript;
    private int mapWidth, mapHeight, roomSize, escapeX, escapeZ, searchOffset;
    public void Main()
    {
        mapWidth = gameObject.GetComponent<RandomMapHandler>().MapWidth;
        mapHeight = gameObject.GetComponent<RandomMapHandler>().MapHeight;
        roomSize = gameObject.GetComponent<RandomMapHandler>().RoomSize;
        searchOffset = 0;
        Boolean found = false;
        while(!found){
            //down to up on left side
            for(int z = searchOffset; z<mapHeight - searchOffset; z++){
                if(randscript.gridHandler[searchOffset, z] != RandomMapHandler.Grid.EMPTY){
                    escapeX = searchOffset;
                    escapeZ = z;
                    found = true;
                }
            }
            //left to right on top side
            for(int x = searchOffset; x<mapWidth - searchOffset; x++){
                if(randscript.gridHandler[x, mapHeight - searchOffset - 1] != RandomMapHandler.Grid.EMPTY){
                    escapeX = x;
                    escapeZ = mapHeight - searchOffset - 1;
                    found = true;
                }
            }
            //down to up on right side
            for(int z = searchOffset; z<mapHeight - searchOffset; z++){
                if(randscript.gridHandler[mapWidth - searchOffset - 1, z] != RandomMapHandler.Grid.EMPTY){
                    escapeX = mapWidth - searchOffset - 1;
                    escapeZ = z;
                    found = true;
                }
            }
            //left to right on bottom side
            for(int x = searchOffset; x<mapWidth - searchOffset; x++){
                if(randscript.gridHandler[x, searchOffset] != RandomMapHandler.Grid.EMPTY){
                    escapeX = x;
                    escapeZ = searchOffset;
                    found = true;
                }
            }
            searchOffset++;    
        }
        Debug.Log("the door coords: (" + escapeX + ", " + escapeZ + ")");
    }   
}
