using UnityEngine;

/*
BY: ALIN
but really by this video
https://www.youtube.com/watch?v=6B7yOnqpK_Y
and this github repo
https://github.com/GarnetKane99/RandomWalkerAlgo_YT

all this script does is store some shit for our walker object
stuff that's required for TileGenWalker
*/
public class TileGenWalkerObject
{
    public Vector2 Position;
    public Vector2 Direction;
    public float ChanceToChange;

    public TileGenWalkerObject(Vector2 pos, Vector2 dir, float chanceToChange){
        Position = pos;
        Direction = dir;
        ChanceToChange = chanceToChange;
    }
}