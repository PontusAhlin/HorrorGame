using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResolutionResize : MonoBehaviour
{
    [SerializeField] RenderTexture renderTexture; //this is needed to actually fetch the render texture
    [SerializeField] int TargetWidthInPixels = 600; //1 IS NONE
    [SerializeField] bool IsPixelationOn = true;
    [SerializeField] string MapToLoadInto = "ShaderCombinedMap";
    // Start is called before the first frame update

    void Start() //WE NEED TO MAKE THIS RUN BEFORE RUNTIME!!!
    {

        ResizeRenderTexture(renderTexture, Screen.width, Screen.height); //runs our resize function for our screen 
        SceneManager.LoadScene(MapToLoadInto); //then loads into BUTTONSCENE.
    }

    
    void ResizeRenderTexture(RenderTexture renderTexture, int width, int height) {
         if (renderTexture) {
            renderTexture.Release();
            if (IsPixelationOn && (TargetWidthInPixels > 0))
            {   int factor = height / TargetWidthInPixels; //swapped because phones have flipped resolution
                width /= factor;
                height /= factor;
            }
            renderTexture.width = width;
            renderTexture.height = height; 
        }
        Debug.Log(width + "x" + height);
    }


}
