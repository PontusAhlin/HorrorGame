using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    // Start is called before the first frame update
    public int width;
    public int height;
    public TileGenWalker.Grid[,] mapgrid;
    void Start()
    {
        for (int i = 1; i <= width ; i++)
            for (int j = 1; j <= height ; j++)
            {
                if (mapgrid[i, j] == TileGenWalker.Grid.FLOOR)
                    DrawRoom(i,j);
            }
        
    }

    void DrawRoom(int x, int y)
    {
        Vector3 pos = new Vector3(x, 0, y);
        Gizmos.DrawCube(pos, Vector3.one);
    }

}
