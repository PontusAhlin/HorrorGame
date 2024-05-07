using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeColour : MonoBehaviour
{
    // SkinnedMeshRenderer is the component of the monster containing its materials
    public SkinnedMeshRenderer[] skinnedMeshRenderer;
    private Material[] materials; // list of Materials applied to monster
    private List<Material> materialsInUse = new List<Material>(); // Array of length 2 which contain the two current materials in use
    public string CurrentColor; 
    Color cyanC = new Color(0f, 1f, 1f, 1f);
    Color blackC = new Color(0f, 0f, 0f, 1f);
    Color blueC = new Color(0f, 0f, 1f, 1f);
    Color grayC = new Color(0.5f, 0.5f, 0.5f, 1f);
    Color magentaC = new Color(1f, 0f, 1f, 1f);

    private class ColorVar 
        {
            public Color color;
            public string name;

            public ColorVar(Color color, string name) 
            {
                this.color = color;
                this.name = name;
            }
        }

    List<ColorVar> colorPallet  = new List<ColorVar>();
    
    


    // Start is called before the first frame update
    void Start()
    {
        // gets skinned Mesh renderer component from child object "Ch11"
        skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        // extracts the materials into an array of type Material 
        materials = skinnedMeshRenderer[0].materials;
        // Makes instances of different colours
        ColorVar cyan = new ColorVar(cyanC, "cyan");
        ColorVar black = new ColorVar(blackC, "black");
        ColorVar blue = new ColorVar(blueC, "blue");
        ColorVar grey = new ColorVar(grayC, "grey");
        ColorVar magenta = new ColorVar(magentaC, "magenta");
        // Adds all instances of ColorVar to colorPallet
        colorPallet.Add(cyan);
        colorPallet.Add(black);
        colorPallet.Add(blue);
        colorPallet.Add(grey);
        colorPallet.Add(magenta);

        

        // Generate a random index within the range of the list
        int randomIndex = Random.Range(0, colorPallet.Count);

        // picks a random color from the colorPallet, then applies this color to material outline color
        materials[0].SetColor("_OutlineColor", colorPallet[randomIndex].color);
        

        CurrentColor = colorPallet[randomIndex].name;
        
    }


}


