using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour

{

    public SkinnedMeshRenderer meshRendererToUse;
    public Material materialToUse;


    // Start is called before the first frame update
    void Start()
    {
        ChangeMaterialToMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeMaterialToMesh()
    {
        meshRendererToUse.material = materialToUse;
    }
}
