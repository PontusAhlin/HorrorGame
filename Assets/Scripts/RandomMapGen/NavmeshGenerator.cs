using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using Unity.AI.Navigation;

public class NavmeshGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshSurface navSurface;
    public void GenerateMesh()

    {
        Debug.Log("NavmeshGenerator script ran");
        navSurface.BuildNavMesh();
    }
}
