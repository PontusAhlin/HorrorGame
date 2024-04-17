using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResolutionResize : MonoBehaviour
{
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] int DivideResolutionBy; //1 IS NONE
    // Start is called before the first frame update

    void Start() //WE NEED TO MAKE THIS RUN BEFORE RUNTIME!!!
    {
        ResizeRenderTexture(renderTexture, Screen.width, Screen.height); //runs our resize function for our screen 
        SceneManager.LoadScene("ButtonScene");
    }

    
    void ResizeRenderTexture(RenderTexture renderTexture, int width, int height) {
         if (renderTexture) {
            renderTexture.Release();
            renderTexture.width = width/DivideResolutionBy;
            renderTexture.height = height/DivideResolutionBy; 
        }
    }


}
