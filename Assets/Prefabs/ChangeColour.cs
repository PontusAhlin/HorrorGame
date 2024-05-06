using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
    // SkinnedMeshRenderer is the component of the monster containing its materials
    public SkinnedMeshRenderer[] skinnedMeshRenderer;
    private Material[] materials; // list of Materials applied to monster
    public Material[] materialsPossibleForUse; // list of Materials which we can to apply to monster
    private List<Material> materialsInUse = new List<Material>(); // Array of length 2 which contain the two current materials in use
    

    // Start is called before the first frame update
    void Start()
    {
        // gets skinned Mesh renderer component from child object "Ch11"
        skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        // extracts the materials into an array of type Material 
        materials = skinnedMeshRenderer[0].materials;
        
        // Adds the first element of the materialsPossibleForUse array to materialsInUse
        materialsInUse.Add(materialsPossibleForUse[0]);
        // Adds the second element of the materialsPossibleForUse array to materialsInUse
        materialsInUse.Add(materialsPossibleForUse[1]);

        // applies the materials in materialsInUse to the monster
        skinnedMeshRenderer[0].SetMaterials(materialsInUse);

        
        
    }

    

    
}
