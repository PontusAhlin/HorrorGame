using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{

    public Material bodyColor;
    

    // Start is called before the first frame update
    void Start()
    {
        setColor(Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setColor(Color c) 
    {
        bodyColor.color = c;
    }
}
