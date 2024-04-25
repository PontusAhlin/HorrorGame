using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShake : MonoBehaviour {

public IEnumerator Shake(float duration , float magnitude)
{
    Vector3 originalPos = transform.localPosition; // samples the camera position

    float elapsed = 0.0f; // elapsed time

    while (elapsed < duration) // for duration
    {
        float x = Random.Range(-0.5f, 0.5f) * magnitude; // we randomly change x and y coordinates 
        float y = Random.Range(-0.5f, 0.5f) * magnitude;

        transform.localPosition = new Vector3(x,y,originalPos.z); // change camera position

        elapsed += Time.deltaTime; // increase elapsed time

        yield return null;
    }

    transform.localPosition = originalPos;
    
}

}