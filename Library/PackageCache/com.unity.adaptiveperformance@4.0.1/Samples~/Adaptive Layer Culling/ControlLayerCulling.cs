using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLayerCulling : MonoBehaviour
{
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        float[] distances = new float[32];
        distances[0] = 0; // Default Layer - 0 will be ignored and left unscaled which results in the far clipping plane
        distances[1] = 200; // TransparentFX Layer - red objects
        distances[2] = 400; // Ignore Raycast Layer - green objects
        distances[3] = 900; // empty Layer
        distances[4] = 1500; // Water Layer - blue objects
        // ignoring the rest for this demo (same as 0)
        camera.layerCullDistances = distances;
    }
}
