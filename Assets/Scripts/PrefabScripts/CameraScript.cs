using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour
{
    private RenderTexture temporaryRT;
    private Camera mainCamera;
 
    private RenderTexture target;
 
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }
 
    private void OnPreRender()
    {
        temporaryRT = RenderTexture.GetTemporary(Screen.width, Screen.height);
        mainCamera.targetTexture = temporaryRT;
    }
 
    private void OnPostRender()
    {
        Graphics.Blit(temporaryRT, target);
        mainCamera.targetTexture = null;
 
        RenderTexture.ReleaseTemporary(temporaryRT);
    }
}
 
