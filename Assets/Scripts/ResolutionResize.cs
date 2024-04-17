using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionResize : MonoBehaviour
{
    [SerializeField] RenderTexture renderTexture;
    // Start is called before the first frame update

    
    void Start()
    {
        Resize(renderTexture, Screen.width, Screen.height);
    }
    
    void Resize(RenderTexture renderTexture, int width, int height) {
         if (renderTexture) {
            renderTexture.Release();
            renderTexture.width = width/2;
            renderTexture.height = height/2; 
        }
    }

}
